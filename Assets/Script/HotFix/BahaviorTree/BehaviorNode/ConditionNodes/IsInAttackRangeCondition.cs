/// <summary>
/// 是否在攻击范围内条件
/// </summary>
public class IsInAttackRangeCondition : BehaviorNode
{
    public BaseSprite Owner;
    
    public IsInAttackRangeCondition(BaseSprite owner)
    {
        Owner = owner;
    }
    
    public override NodeState Evaluate()
    {
        return NodeState.Failure;
    }
}