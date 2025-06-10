
// 并行节点 同时执行多个子节点
public class ParallelNode : CompositeNode
{
    public override NodeState Evaluate()
    {
        bool anyChildRunning = false;
        bool anyChildFailure = false;

        foreach (var child in children)
        {
            switch (child.Evaluate())
            {
                case NodeState.Running:
                    anyChildRunning = true;
                    break;
                case NodeState.Failure:
                    anyChildFailure = true;
                    break;
            }
        }

        if (anyChildFailure)
        {
            state = NodeState.Failure;
            return state;
        }
        
        state = anyChildRunning ? NodeState.Running : NodeState.Success;
        return state;
    }
}