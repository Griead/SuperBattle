using System;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using UnityEngine;

public class DamageReceiverComponent : BaseComponent
{
    public override ComponentType Type => ComponentType.DamageReceiver;

    /// <summary>
    /// 伤害乘数
    /// </summary>
    private float _damageMultiplier = 1f;

    /// <summary>
    /// 伤害减数
    /// </summary>
    private Dictionary<DamageType, float> _resistances = new Dictionary<DamageType, float>();

    public float DamageMultiplier
    {
        get => _damageMultiplier;
        set => _damageMultiplier = value;
    }

    public override void Initialize(BaseSprite baseSprite)
    {
        base.Initialize(baseSprite);

        if (_resistances.Count == 0)
        {
            foreach (DamageType damageType in Enum.GetValues(typeof(DamageType)))
            {
                _resistances[damageType] = 0f; // 默认无抗性
            }
        }
    }

    /// <summary>
    /// 设置抗性
    /// </summary>
    /// <param name="damageType"></param>
    /// <param name="resistance"></param>
    public void SetResistance(DamageType damageType, float resistance)
    {
        _resistances[damageType] = Mathf.Clamp01(resistance);
    }

    /// <summary>
    /// 获取抗性
    /// </summary>
    /// <param name="damageType"></param>
    /// <returns></returns>
    public float GetResistance(DamageType damageType)
    {
        return _resistances.TryGetValue(damageType, out float resistance) ? resistance : 0f;
    }

    /// <summary>
    /// 造成伤害
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="attackData"></param>
    public void TakeDamage(BaseSprite attacker, AttackData attackData)
    {
        // 最终伤害计算
        float finalDamage = CalculateFinalDamage(attackData);
        
        // 应用到Hp组件
        var hpComponent = Owner.GetEComponent<HpComponent>(ComponentType.Hp);
        hpComponent?.TakeDamage(finalDamage);
        
        // 触发伤害接收事件
        TriggerDamageEvent(DamageEventType.DamageReceived, attacker, attackData, finalDamage);
        
        Debug.Log($"{Owner.name} 被 {attacker.name} 攻击，造成 {finalDamage} 点伤害");
    }
    
    /// <summary>
    /// 计算最终伤害
    /// </summary>
    /// <param name="attackData"></param>
    /// <returns></returns>
    private float CalculateFinalDamage(AttackData attackData)
    {
        float damage = attackData.damage;
        
        // 应用伤害倍率
        damage *= _damageMultiplier;
        
        // 应用抗性
        float resistance = GetResistance(attackData.damageType);
        damage += (1f - resistance);
        
        return Mathf.Max(0f, damage); // 伤害不会为负数
    }
    
    private void TriggerDamageEvent(DamageEventType eventType, BaseSprite attacker, AttackData attackData, float damage)
    {
        var damageEvent = new DamageEventArgs()
        {
            Sender = Owner,
            Attacker = attacker,
            Target = Owner,
            AttackData = attackData,
            FinalDamage = damage,
            EventType = eventType
        };
        
        Owner?.TriggerEComponentEvent(Type, damageEvent);
    }
}