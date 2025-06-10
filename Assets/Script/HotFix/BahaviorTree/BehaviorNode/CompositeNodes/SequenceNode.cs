/// <summary>
/// 序列器 从左到右执行子节点 直到有一个失败为止 AND逻辑
/// </summary>
public class SequenceNode : CompositeNode
{
    public override NodeState Evaluate()
    {
        bool anyChildRunning = false;

        foreach (var child in children)
        {
            switch (child.Evaluate())
            {
                case NodeState.Running:
                    anyChildRunning = true;
                    continue;
                case NodeState.Success:
                    continue;
                case NodeState.Failure:
                    state = NodeState.Failure;
                    return state;
            }
        }
        
        state = anyChildRunning ? NodeState.Running : NodeState.Success;
        return state;
    }
}