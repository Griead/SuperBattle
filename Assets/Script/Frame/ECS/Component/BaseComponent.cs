// 基础组件抽象类

public abstract class BaseComponent : IComponent
{
    public abstract ComponentType Type { get; }
    public BaseSprite Owner { get; set; }

    public virtual void Initialize(BaseSprite baseSprite)
    {
    }

    public virtual void Update(float deltaTime)
    {
    }

    public virtual void FixedUpdate(float deltaTime)
    {
    }

    public virtual void OnDestroy()
    {
    }
}