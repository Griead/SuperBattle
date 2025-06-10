using UnityEngine;

public interface IComponent
{
    public abstract ComponentType Type { get; }
    public BaseSprite Owner { get; set; }

    void Initialize(BaseSprite baseSprite);
    void Update(float deltaTime);
    void FixedUpdate(float deltaTime);
    void OnRelease();
}