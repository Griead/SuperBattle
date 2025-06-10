
/// <summary>
/// 反转器 反转子节点的返回值
/// </summary>
public class InverterNode : DecoratorNode
{
    public override NodeState Evaluate()
    {
        switch (child.Evaluate())
        {
            case NodeState.Running:
                state = NodeState.Running;
                break;
            case NodeState.Success:
                state = NodeState.Failure;
                break;
            case NodeState.Failure:
                state = NodeState.Success;
                break;
        }

        return state;
    }
}