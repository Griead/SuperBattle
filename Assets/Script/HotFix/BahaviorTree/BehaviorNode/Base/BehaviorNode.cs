/// <summary>
/// 行为树节点基类
/// </summary>
public abstract class BehaviorNode
{
    protected NodeState state;
    public NodeState State => state;
    
    /// <summary>
    /// 评估
    /// </summary>
    /// <returns></returns>
    public abstract NodeState Evaluate();
}