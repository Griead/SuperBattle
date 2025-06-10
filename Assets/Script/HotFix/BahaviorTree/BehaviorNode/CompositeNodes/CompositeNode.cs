using System.Collections.Generic;

/// <summary>
/// 复合节点基类
/// </summary>
public abstract class CompositeNode : BehaviorNode
{
    protected List<BehaviorNode> children = new List<BehaviorNode>();

    public void AddChild(BehaviorNode child)
    {
        children.Add(child);
    }
}