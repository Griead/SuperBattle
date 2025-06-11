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
        
        this.AddEComponent<RoleMoveComponent>(ComponentType.RoleMoveInput, OnMoveEvent)
            .AddEComponent( new CameraFollowComponent() { MaxSpeed = _speed })
            .AddEComponent<HpComponent>(ComponentType.Hp, OnHpEvent);
    }

    private void OnMoveEvent(ComponentEventArgs args)
    {
        var moveArgs = args as RoleMoveInputEventArgs;

        moveArgs.Sender.transform.position += new Vector3(moveArgs.Direction.x, moveArgs.Direction.y) * Speed * moveArgs.DeltaTime;
    }

    private void OnHpEvent(ComponentEventArgs args)
    {
        var hpArgs = args as HpEventArgs;
        
        switch (hpArgs.EventType)
        {
            case HpEventType.TakeDamage:
                Debug.Log($"角色{hpArgs.Sender.name}被攻击，当前血量{hpArgs.CurrentHp}");
                break;
            case HpEventType.Heal:
                Debug.Log($"角色{hpArgs.Sender.name}被治疗，当前血量{hpArgs.CurrentHp}");
                break;
            case HpEventType.Die:
                Debug.Log($"角色{hpArgs.Sender.name}死亡");
                break;
            case HpEventType.HpChanged:
                Debug.Log($"角色{hpArgs.Sender.name}血量变化，当前血量{hpArgs.CurrentHp}");
                break;
        }
    }
}