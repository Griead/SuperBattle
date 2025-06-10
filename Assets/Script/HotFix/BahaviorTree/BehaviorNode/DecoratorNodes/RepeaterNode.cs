/// <summary>
/// 重复节点 重复执行
/// </summary>
public class RepeaterNode : DecoratorNode
{
    private int repeatCount;
    private int currentCount = 0;
    
    public RepeaterNode(int repeatCount = -1)
    {
        this.repeatCount = repeatCount;
    }
    
    public override NodeState Evaluate()
    {
        if (repeatCount != -1 && currentCount >= repeatCount)
        {
            state = NodeState.Success;
            return state;
        }

        switch (child.Evaluate())
        {
            case NodeState.Running:
                state = NodeState.Running;
                break;
            case NodeState.Success:
            case NodeState.Failure:
                {
                    currentCount++;
                    if (repeatCount != -1 && currentCount >= repeatCount)
                    {
                        state = NodeState.Success;
                    }
                    else
                    {
                        state = NodeState.Running;
                    }
                    break;
                }
        }

        return state;
    }
}