/// <summary>
/// 选择器节点 从左到右执行子节点 直到有一个成功为止 Or逻辑
/// </summary>
public class SelectorNode : CompositeNode
{
    public override NodeState Evaluate()
    {
        foreach (var child in children)
        {
            switch (child.Evaluate())
            {
                case NodeState.Running:
                    state = NodeState.Running;
                    return state;
                case NodeState.Success:
                    state = NodeState.Success;
                    return state;
                case NodeState.Failure:
                    state = NodeState.Failure;
                    continue;
            }
        }
        
        state = NodeState.Failure;
        return state;
    }
}