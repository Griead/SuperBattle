using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageDealerComponent : BaseComponent
{
    public override ComponentType Type => ComponentType.DamageDealer;

    private List<AttackRecord> _activeAttacks = new List<AttackRecord>();
    private List<Collider2D> _detectedTargets = new List<Collider2D>();

    private Transform _attackPoint;
    private AttackRangeData _rangeData;
    private LayerMask _targetLayers;

    public Transform AttackPoint
    {
        get => _attackPoint;
        set => _attackPoint = value;
    }

    public AttackRangeData RangeData
    {
        get => _rangeData;
        set => _rangeData = value;
    }
    
    public override void Initialize(BaseSprite baseSprite)
    {
        base.Initialize(baseSprite);
        
        // 如果没有设置攻击点，则使用Owner的transform
        if (_attackPoint == null)
        {
            _attackPoint = Owner.transform;
        }

        if (_rangeData == null)
        {
            _rangeData = AttackRangeData.Circle(2f);
        }
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        
        // 更新活跃攻击
        UpdateActiveAttacks();
        
        // 检测攻击目标
        DetectTargets();
        
        // 处理攻击命中
        ProcessAttackHits();
    }

    /// <summary>
    /// 发起攻击
    /// </summary>
    /// <param name="attackData"></param>
    public void StartAttack(AttackData attackData)
    {
        var attackRecord = new AttackRecord(attackData);
        _activeAttacks.Add(attackRecord);
        
        // 触发攻击开始事件
        // TriggerDamageEvent(DamageEventType.AttStart, null, attackData);
        
        Debug.Log($"开始攻击: {attackData.attackId}, 伤害: {attackData.damage}, 范围: {attackData.rangeData.rangeType}");
    }
    
    /// 便捷的攻击方法
    public void Attack(float damage, AttackRangeData rangeData = null, DamageType damageType = DamageType.Normal, 
        string attackId = null, int maxHits = 1, float interval = 0.5f)
    {
        if (string.IsNullOrEmpty(attackId))
        {
            attackId = $"attack_{Time.time}";
        }

        if (rangeData == null)
        {
            rangeData = RangeData;
        }

        var attackData = new AttackData(attackId, damage, damageType)
        {
            maxHitCount = maxHits,
            hitInterval = interval,
            duration = interval * maxHits + 0.5f,
            layerMask = _targetLayers,
            rangeData = rangeData
        };

        StartAttack(attackData);
    }
    
    // 便捷攻击方法
    public void CircleAttack(float damage, float radius, DamageType damageType = DamageType.Normal)
    {
        var rangeData = AttackRangeData.Circle(radius);
        Attack(damage, rangeData, damageType, "circle_attack");
    }

    public void SectorAttack(float damage, float radius, float angle, float rotation = 0f, DamageType damageType = DamageType.Normal)
    {
        var rangeData = AttackRangeData.Sector(radius, angle, rotation);
        Attack(damage, rangeData, damageType, "sector_attack");
    }

    public void RectangleAttack(float damage, float width, float height, float rotation = 0f, DamageType damageType = DamageType.Normal)
    {
        var rangeData = AttackRangeData.Rectangle(width, height, rotation);
        Attack(damage, rangeData, damageType, "rectangle_attack");
    }

    public void ConeAttack(float damage, float range, float angle, float rotation = 0f, DamageType damageType = DamageType.Normal)
    {
        var rangeData = AttackRangeData.Cone(range, angle, rotation);
        Attack(damage, rangeData, damageType, "cone_attack");
    }

    /// <summary>
    /// 更新活跃的攻击
    /// </summary>
    private void UpdateActiveAttacks()
    {
        for (int i = _activeAttacks.Count - 1; i >= 0; i--)
        {
            var attack = _activeAttacks[i];
            
            // 检测攻击是否过期
            if (Time.time - attack.startTime > attack.attackData.duration)
            {
                attack.isActive = false;
                // TriggerDamageEvent(DamageEventType.AttackEnd, null, attack.attackData);
                _activeAttacks.RemoveAt(i);
            }
        }
    }

    /// <summary>
    ///  检测攻击目标
    /// </summary>
    private void DetectTargets()
    {
        _detectedTargets.Clear();

        foreach (var attack in _activeAttacks)
        {
            if(!attack.isActive)
                continue;

            var targets = DetectTargetsInRange(attack.attackData.rangeData, attack.attackData.layerMask);
            foreach (var target in targets)
            {
                if (!_detectedTargets.Contains(target))
                {
                    _detectedTargets.Add(target);
                }
            }
        }
    }

    /// <summary>
    /// 探测目标
    /// </summary>
    /// <param name="rangeData"></param>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    private List<Collider2D> DetectTargetsInRange(AttackRangeData rangeData, LayerMask layerMask)
    {
        var targets = new  List<Collider2D>();
        Vector2 attackPosition = (Vector2)_attackPoint.position + rangeData.offset;

        switch (rangeData.rangeType)
        {
            case AttackRangeType.Circle:
                targets.AddRange(DetectCircleTargets(attackPosition, rangeData.range, layerMask));
                break;
                
            case AttackRangeType.Sector:
                targets.AddRange(DetectSectorTargets(attackPosition, rangeData.range, rangeData.angle, rangeData.rotation, layerMask));
                break;
                
            case AttackRangeType.Rectangle:
                targets.AddRange(DetectRectangleTargets(attackPosition, rangeData.width, rangeData.height, rangeData.rotation, layerMask));
                break;
                
            case AttackRangeType.Cone:
                targets.AddRange(DetectConeTargets(attackPosition, rangeData.range, rangeData.angle, rangeData.rotation, layerMask));
                break;
        }

        targets.RemoveAll(target => target.transform.GetComponent<BaseSprite>() == Owner);
        return targets;
    }

    /// <summary>
    /// 圆形范围检测
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    private List<Collider2D> DetectCircleTargets(Vector2 center, float radius, LayerMask layerMask)
    {
        var colliders = Physics2D.OverlapCircleAll(center, radius, layerMask);
        return colliders.ToList();
    }

    /// <summary>
    /// 扇形范围检测
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <param name="angle"></param>
    /// <param name="rotation"></param>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    private List<Collider2D> DetectSectorTargets(Vector2 center, float radius, float angle, float rotation, LayerMask layerMask)
    {
        var allTargets = Physics2D.OverlapCircleAll(center, radius, layerMask);
        var sectorTargets = new List<Collider2D>();

        Vector2 forward = GetDirectionFromRotation(rotation);
        float halfAngle = angle * 0.5f;

        foreach (var target in allTargets)
        {
            Vector2 dirToTarget = ((Vector2)target.transform.position - center).normalized;
            float angleToTarget = Vector2.Angle(forward, dirToTarget);
            
            if (angleToTarget <= halfAngle)
            {
                sectorTargets.Add(target);
            }
        }

        return sectorTargets;
    }
    
    // 矩形范围检测
    private List<Collider2D> DetectRectangleTargets(Vector2 center, float width, float height, float rotation, LayerMask layerMask)
    {
        // 创建一个BoxCollider2D的查询
        var size = new Vector2(width, height);
        var colliders = Physics2D.OverlapBoxAll(center, size, rotation, layerMask);
        return new List<Collider2D>(colliders);
    }

    /// <summary>
    /// 锥形范围检测
    /// </summary>
    /// <param name="center"></param>
    /// <param name="range"></param>
    /// <param name="angle"></param>
    /// <param name="rotation"></param>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    private List<Collider2D> DetectConeTargets(Vector2 center, float range, float angle, float rotation, LayerMask layerMask)
    {
        var allTargets = Physics2D.OverlapCircleAll(center, range, layerMask);
        var coneTargets = new List<Collider2D>();

        Vector2 forward = GetDirectionFromRotation(rotation);
        float halfAngle = angle * 0.5f;

        foreach (var target in allTargets)
        {
            Vector2 dirToTarget = (Vector2)target.transform.position - center;
            float distance = dirToTarget.magnitude;
            dirToTarget.Normalize();

            float angleToTarget = Vector2.Angle(forward, dirToTarget);

            if (angleToTarget <= halfAngle && distance <= range)
            {
                coneTargets.Add(target);
            }
        }

        return coneTargets;
    }

    /// <summary>
    /// 从旋转角度获取方向向量
    /// </summary>
    /// <param name="rotation"></param>
    /// <returns></returns>
    private Vector2 GetDirectionFromRotation(float rotation)
    {
        float rad = rotation * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    /// <summary>
    /// 处理攻击命中
    /// </summary>
    private void ProcessAttackHits()
    {
        foreach (var attack in _activeAttacks)
        {
            if(!attack.isActive)
                continue;
            
            var targets = DetectTargetsInRange(attack.attackData.rangeData, attack.attackData.layerMask);
            for (int i = 0; i < targets.Count; i++)
            {
                
            }
        }
    }
}