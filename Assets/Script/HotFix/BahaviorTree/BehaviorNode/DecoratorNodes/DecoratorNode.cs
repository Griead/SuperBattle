/// <summary>
/// 装饰节点基类
/// </summary>
public abstract class DecoratorNode : BehaviorNode
{
    protected BehaviorNode child;

    public void SetChild(BehaviorNode child)
    {
        this.child = child;
    }
}