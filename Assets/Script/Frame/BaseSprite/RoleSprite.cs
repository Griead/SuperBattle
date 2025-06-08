using UnityEngine;

public class RoleSprite : BaseSprite
{
    /// <summary>
    /// 速度
    /// </summary>
    private float _speed = 50f;
    public float Speed => _speed;
    
    protected override void OnStart()
    {
        base.OnStart();
        
        this.AddEComponent<RoleMoveComponent>(ComponentType.RoleMoveInput, OnMove)
            .AddEComponent<CameraFollowComponent>(ComponentType.CameraFollow);
    }

    private void OnMove(ComponentEventArgs args)
    {
        var moveArgs = args as RoleMoveInputEventArgs;

        moveArgs.Sender.transform.position += new Vector3(moveArgs.Direction.x, moveArgs.Direction.y) * Speed * moveArgs.DeltaTime;
    }
}