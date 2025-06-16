using UnityEngine;

public enum DamageType
{
    Normal
}

public class AttackData
{
    /// <summary>
    /// 攻击唯一ID
    /// </summary>
    public string attackId;

    /// <summary>
    /// 伤害值
    /// </summary>
    public float damage;

    /// <summary>
    /// 伤害类型
    /// </summary>
    public DamageType damageType;

    /// <summary>
    /// 最大命中次数（同一目标）
    /// </summary>
    public int maxHitCount;

    /// <summary>
    /// 命中间隔时间
    /// </summary>
    public float hitInterval;

    /// <summary>
    /// 攻击持续时间
    /// </summary>
    public float duration;
    
    /// <summary>
    /// 目标层级
    /// </summary>
    public LayerMask layerMask;
    
    /// <summary>
    /// 是否能够重复命中同一目标
    /// </summary>
    public bool canHitSameTarget;
    
    /// <summary>
    /// 攻击范围数据
    /// </summary>
    public AttackRangeData rangeData;

    public AttackData(string id, float dmg, AttackRangeData rangeData, DamageType type = DamageType.Normal)
    {
        attackId = id;
        damage = dmg;
        damageType = type;
        maxHitCount = 1;
        hitInterval = 0.5f;
        duration = 1f;
        layerMask = -1;
        canHitSameTarget = false;
        rangeData = rangeData; // 默认圆形范围
    }
}