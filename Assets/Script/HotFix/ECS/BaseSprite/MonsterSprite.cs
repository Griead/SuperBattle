
using Pathfinding;

public class MonsterSprite : BaseSprite, IPoolable
{
    protected override void OnStart()
    {
        base.OnStart();
        
        SetData();
    }

    private void SetData()
    {
        this.AddEComponent(new PathFindingComponent()
        {
            Target = GlobalManager.Instance.GetModel<GameGlobalManager>().GetRoleSprite().transform,
            Seeker = this.GetComponent<Seeker>(),
            Speed = 20f
        });
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
}