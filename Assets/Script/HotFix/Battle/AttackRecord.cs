using System.Collections.Generic;
using UnityEngine;

public class AttackRecord
{
    public string attackId;
    public AttackData attackData;
    public float startTime;
    
    /// <summary> 每个目标的命中次数 </summary>
    public Dictionary<BaseSprite, int> hitCounts;

    /// <summary> 每个目标的最近一次命中时间 </summary>
    public Dictionary<BaseSprite, float> lastHitTimes;

    public bool isActive;
    
    public AttackRecord(AttackData data)
    { 
        attackId = data.attackId;
        attackData = data;
        startTime = Time.time;
        hitCounts = new Dictionary<BaseSprite, int>();
        lastHitTimes = new Dictionary<BaseSprite, float>();
        isActive = true;
    }

    public bool CanHitTarget(BaseSprite target)
    {
        if (!isActive)
            return false;

        // 还在持续时间内
        if (Time.time - startTime > attackData.duration)
        {
            isActive = false;
            return false;
        }

        // 重复命中同一目标
        if (!attackData.canHitSameTarget && hitCounts.ContainsKey(target))
        {
            return false;
        }

        // 命中间隔
        if (lastHitTimes.TryGetValue(target, out var lastHitTime))
        {
            if (Time.time - lastHitTime < attackData.hitInterval)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 记录攻击
    /// </summary>
    /// <param name="target"></param>
    public void RecordHit(BaseSprite target)
    {
        if (!hitCounts.ContainsKey(target))
        {
            hitCounts[target] = 0;
        }
        
        hitCounts[target]++;
        lastHitTimes[target] = Time.time;
    }
}