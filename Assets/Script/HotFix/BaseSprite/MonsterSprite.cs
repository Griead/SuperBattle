using UnityEngine;
using UnityEngine.AI;

public class MonsterSprite : BaseSprite, IPoolable
{
    private NavMeshAgent Agent;
    public BaseSprite Target;
    protected override void OnStart()
    {
        base.OnStart();
        
        SetData();
    }

    private void SetData()
    {
        var pathFindingComponent = new PathFindingComponent()
        {
            Agent = this.Agent,
        };
        
        pathFindingComponent.SetTarget(Target.transform);
        this.AddEComponent(pathFindingComponent);
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