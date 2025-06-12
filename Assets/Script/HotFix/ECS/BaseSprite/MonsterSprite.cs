
using Pathfinding;
using UnityEngine;

public class MonsterSprite : BaseSprite, IPoolable
{
    protected override void OnStart()
    {
        base.OnStart();
        
        SetData();
    }

    private void SetData()
    {
        PathFindingComponent pathFindingComponent = new PathFindingComponent()
        {
            Target = GlobalManager.Instance.GetModel<GameGlobalManager>().GetRoleSprite().transform,
            Seeker = this.GetComponent<Seeker>(),
            Speed = 20f
        };
        
        this.AddEComponent(pathFindingComponent)
            .AddEComponent<HpComponent>(ComponentType.Hp, OnHpEvent)
            .AddEComponent(new CampComponent(CampType.Monster))
            ;
    }

    public void OnSpawn()
    {
        SetData();
    }

    public void OnDespawn()
    {
        RemoveEComponent(ComponentType.PathFinding);
    }

    protected override void OnRelease()
    {
        base.OnRelease();
    }
    
    private void OnHpEvent(ComponentEventArgs args)
    {
        var hpArgs = args as HpEventArgs;
        
        switch (hpArgs.EventType)
        {
            case HpEventType.TakeDamage:
                Debug.Log($"怪物{hpArgs.Sender.name}被攻击，当前血量{hpArgs.CurrentHp}");
                break;
            case HpEventType.Heal:
                Debug.Log($"怪物{hpArgs.Sender.name}被治疗，当前血量{hpArgs.CurrentHp}");
                break;
            case HpEventType.Die:
                Debug.Log($"怪物{hpArgs.Sender.name}死亡");
                break;
            case HpEventType.HpChanged:
                Debug.Log($"怪物{hpArgs.Sender.name}血量变化，当前血量{hpArgs.CurrentHp}");
                break;
        }
    }
}