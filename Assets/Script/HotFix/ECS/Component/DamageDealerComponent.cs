using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageDealerComponent : BaseComponent
{
    public override ComponentType Type => ComponentType.DamageDealer;

    private List<AttackRecord> _activeAttacks = new List<AttackRecord>();
    private List<Collider2D> _detectedTargets = new List<Collider2D>();

    // 组件创建时定义
    private LayerMask _targetLayers = -1;

    public override void Initialize(BaseSprite baseSprite)
    {
        base.Initialize(baseSprite);
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
        TriggerDamageEvent(DamageEventType.AttackStart, null, attackData);

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

        var attackData = new AttackData(attackId, damage, rangeData, damageType)
        {
            maxHitCount = maxHits,
            hitInterval = interval,
            duration = interval * maxHits + 0.5f,
            layerMask = _targetLayers,
        };

        StartAttack(attackData);
    }

    /// 便捷攻击方法
    public void CircleAttack(Transform center, float damage, float radius, DamageType damageType = DamageType.Normal)
    {
        var rangeData = AttackRangeData.Circle(center, radius);
        Attack(damage, rangeData, damageType, "circle_attack");
    }

    public void SectorAttack(Transform center, float damage, float radius, float angle, float rotation = 0f, DamageType damageType = DamageType.Normal)
    {
        var rangeData = AttackRangeData.Sector(center, radius, angle, rotation);
        Attack(damage, rangeData, damageType, "sector_attack");
    }

    public void RectangleAttack(Transform center, float damage, float width, float height, float rotation = 0f, DamageType damageType = DamageType.Normal)
    {
        var rangeData = AttackRangeData.Rectangle(center, width, height, rotation);
        Attack(damage, rangeData, damageType, "rectangle_attack");
    }

    public void ConeAttack(Transform center, float damage, float range, float angle, float rotation = 0f, DamageType damageType = DamageType.Normal)
    {
        var rangeData = AttackRangeData.Cone(center, range, angle, rotation);
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
                TriggerDamageEvent(DamageEventType.AttackEnd, null, attack.attackData);
                _activeAttacks.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 检测攻击目标
    /// </summary>
    private void DetectTargets()
    {
        _detectedTargets.Clear();

        foreach (var attack in _activeAttacks)
        {
            if (!attack.isActive)
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
        var targets = new List<Collider2D>();
        Vector2 attackPosition = (Vector2)rangeData.center.position + rangeData.offset;

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
            if (!attack.isActive)
                continue;

            var targets = DetectTargetsInRange(attack.attackData.rangeData, attack.attackData.layerMask);

            foreach (var targetCollider in targets)
            {
                var targetSprite = targetCollider.GetComponent<BaseSprite>();
                if (targetSprite == null)
                    continue;

                if (attack.CanHitTarget(targetSprite))
                {
                    attack.RecordHit(targetSprite);
                    
                    // 触发攻击命中事件
                    TriggerDamageEvent(DamageEventType.AttackHit, targetSprite, attack.attackData);
                    
                    // 造成伤害
                    DealDamageToTarget(targetSprite, attack.attackData);
                }
            }
        }
    }

    /// <summary>
    /// 对目标造成伤害
    /// </summary>
    private void DealDamageToTarget(BaseSprite target, AttackData attackData)
    {
        var receiverComponent = target.GetEComponent<DamageReceiverComponent>(ComponentType.DamageReceiver);
        if (receiverComponent != null)
        {
            receiverComponent.TakeDamage(Owner, attackData);
        }
        else
        {
            // 如果目标没有伤害接收器组件，则对HP组件造成伤害
            var hpComponent = target.GetEComponent<HpComponent>(ComponentType.Hp);
            hpComponent?.TakeDamage(attackData.damage);
        }

        // 触发伤害输出事件
        TriggerDamageEvent(DamageEventType.DamageDealt, target, attackData, attackData.damage);

        Debug.Log($"{Owner.name} 对 {target.name} 造成 {attackData.damage} 点 {attackData.damageType} 伤害");
    }

    /// <summary>
    /// 触发伤害事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="target"></param>
    /// <param name="attackData"></param>
    /// <param name="finalDamage"></param>
    private void TriggerDamageEvent(DamageEventType eventType, BaseSprite target, AttackData attackData, float finalDamage = 0)
    {
        var args = new DamageEventArgs()
        {
            Attacker = Owner,
            Target = target,
            AttackData = attackData,
            FinalDamage = finalDamage,
            EventType = eventType
        };

        Owner?.TriggerEComponentEvent(Type, args);
    }

    public override void OnRelease()
    {
        _activeAttacks.Clear();
        _detectedTargets.Clear();
        base.OnRelease();
    }

    //
    // private void OnDrawGizmosSelected()
    // {
    //     if (rangeData.center == null) return;
    //
    //     // 绘制默认范围 
    //     if (_rangeData != null)
    //     {
    //         DrawAttackRange(_rangeData, Color.red);
    //     }
    //
    //     // 绘制活跃攻击范围
    //     foreach (var attack in _activeAttacks)
    //     {
    //         if (attack.isActive)
    //         {
    //             DrawAttackRange(attack.attackData.rangeData, Color.yellow);
    //         }
    //     }
    // }
    //
    // /// <summary>
    // /// 绘制攻击范围
    // /// </summary>
    // /// <param name="rangeData"></param>
    // /// <param name="color"></param>
    // private void DrawAttackRange(AttackRangeData rangeData, Color color)
    // {
    //     Gizmos.color = color;
    //     Vector3 center = _attackPoint.position + (Vector3)rangeData.offset;
    //
    //     switch (rangeData.rangeType)
    //     {
    //         case AttackRangeType.Circle:
    //             DrawCircleGizmo(center, rangeData.range);
    //             break;
    //
    //         case AttackRangeType.Sector:
    //             DrawSectorGizmo(center, rangeData.range, rangeData.angle, rangeData.rotation);
    //             break;
    //
    //         case AttackRangeType.Rectangle:
    //             DrawRectangleGizmo(center, rangeData.width, rangeData.height, rangeData.rotation);
    //             break;
    //
    //         case AttackRangeType.Cone:
    //             DrawConeGizmo(center, rangeData.range, rangeData.angle, rangeData.rotation);
    //             break;
    //     }
    // }
    //
    // // 绘制圆形
    // private void DrawCircleGizmo(Vector3 center, float radius)
    // {
    //     DrawSectorGizmo(center, radius, 360, 0);
    // }
    //
    // // 绘制扇形
    // private void DrawSectorGizmo(Vector3 center, float radius, float angle, float rotation)
    // {
    //     Vector3 forward = GetDirectionFromRotation(rotation);
    //     float halfAngle = angle * 0.5f;
    //
    //     Vector3 leftBound = Quaternion.Euler(0, 0, -halfAngle) * forward * radius;
    //     Vector3 rightBound = Quaternion.Euler(0, 0, halfAngle) * forward * radius;
    //
    //     Gizmos.DrawLine(center, center + leftBound);
    //     Gizmos.DrawLine(center, center + rightBound);
    //
    //     // 绘制弧线
    //     int segments = 20;
    //     for (int i = 0; i < segments; i++)
    //     {
    //         float currentAngle = -halfAngle + (angle / segments) * i;
    //         float nextAngle = -halfAngle + (angle / segments) * (i + 1);
    //
    //         Vector3 currentPoint = center + Quaternion.Euler(0, 0, rotation + currentAngle) * Vector3.right * radius;
    //         Vector3 nextPoint = center + Quaternion.Euler(0, 0, rotation + nextAngle) * Vector3.right * radius;
    //
    //         Gizmos.DrawLine(currentPoint, nextPoint);
    //     }
    // }
    //
    // // 绘制矩形
    // private void DrawRectangleGizmo(Vector3 center, float width, float height, float rotation)
    // {
    //     Vector3[] corners = new Vector3[4];
    //     float halfWidth = width * 0.5f;
    //     float halfHeight = height * 0.5f;
    //
    //     corners[0] = new Vector3(-halfWidth, -halfHeight, 0);
    //     corners[1] = new Vector3(halfWidth, -halfHeight, 0);
    //     corners[2] = new Vector3(halfWidth, halfHeight, 0);
    //     corners[3] = new Vector3(-halfWidth, halfHeight, 0);
    //
    //     // 应用旋转
    //     for (int i = 0; i < 4; i++)
    //     {
    //         corners[i] = Quaternion.Euler(0, 0, rotation) * corners[i] + center;
    //     }
    //
    //     // 绘制边框
    //     for (int i = 0; i < 4; i++)
    //     {
    //         Gizmos.DrawLine(corners[i], corners[(i + 1) % 4]);
    //     }
    // }
    //
    // // 绘制锥形
    // private void DrawConeGizmo(Vector3 center, float range, float angle, float rotation)
    // {
    //     DrawSectorGizmo(center, range, angle, rotation); // 锥形和扇形绘制方式相同
    // }
}