using UnityEngine;

public class RoleMoveComponent : BaseComponent
{
    public override ComponentType Type => ComponentType.RoleMoveInput;
    
    public override void Initialize(BaseSprite baseSprite)
    {
        base.Initialize(baseSprite);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0 || moveY != 0)
        {
            var args = new RoleMoveInputEventArgs
            {
                Sender = Owner,
                Direction = new Vector2(moveX, moveY),
                DeltaTime = deltaTime,
            };
        
            Owner.TriggerEComponentEvent(ComponentType.RoleMoveInput, args);
        }
    }
}