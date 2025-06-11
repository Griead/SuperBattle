using System;
using UnityEngine;

public class HpComponent : BaseComponent
{
    public override ComponentType Type => ComponentType.Hp;

    private float _maxHp = 100f;
    private float _curHp = 100f;
    private bool _isDead = false;

    public float MaxHp
    {
        get => _maxHp;
        set
        {
            var oldMaxHp = _maxHp;
            _maxHp = Mathf.Max(0, value);
        }
    }

    public float CurHp
    {
        get => _curHp;
        private set
        {
            var oldHp = _curHp;
            _curHp = Mathf.Clamp(value, 0, _maxHp);

            if (Mathf.Abs(oldHp - _curHp) > 0.001f)
            {
                // 有血量更改
                Debug.Log("血量更改");
            }

            // 检查是否死亡
            if (_curHp <= 0 && !_isDead)
            {
                _isDead = true;
            }
            else if (_curHp > 0 && _isDead)
            {
                _isDead = false; // 复活
            }
        }
    }

    public bool IsDead => _isDead;
    public float HpPercentage => _maxHp > 0 ? _curHp / _maxHp : 0f;

    public override void Initialize(BaseSprite baseSprite)
    {
        base.Initialize(baseSprite);

        _curHp = _maxHp;
        _isDead = false;
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        if (_isDead || damage <= 0)
            return;

        var actualDamage = Mathf.Min(damage, _curHp);
        CurHp -= actualDamage;

        TriggerHpEvent(HpEventType.TakeDamage, damageAmount: actualDamage);
    }

    /// <summary>
    /// 治疗
    /// </summary>
    /// <param name="healAmount"></param>
    public void Heal(float healAmount)
    {
        if (healAmount <= 0)
            return;

        var actualHeal = Mathf.Min(healAmount, _maxHp - _curHp);
        CurHp += actualHeal;

        TriggerHpEvent(HpEventType.Heal, healAmount: actualHeal);
    }

    private void TriggerHpEvent(HpEventType eventType, float damageAmount = 0f, float healAmount = 0f)
    {
        var args = new HpEventArgs
        {
            CurrentHp = _curHp,
            MaxHp = _maxHp,
            DamageAmount = damageAmount,
            HealAmount = healAmount,
            EventType = eventType
        };

        Owner?.TriggerEComponentEvent(Type, args);
    }

    public override void OnRelease()
    {
        base.OnRelease();
    }
}