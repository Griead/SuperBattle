using UnityEngine;
using UnityEngine.AI;

public class PathFindingComponent : BaseComponent
{
    public override ComponentType Type => ComponentType.PathFinding;

    public NavMeshAgent Agent { get; set; }
    
    // 寻路目标
    private Transform Target { get; set; }

    // 最小距离
    private float MinDistance { get; set; } = 10;

    public void SetTarget(Transform target)
    {
        Target = target;
    }
    
    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if (Target is null)
            return;

        if (Vector3.Distance(Owner.transform.position, Target.position) < MinDistance)
            return;

        Agent.SetDestination(Target.position);
    }
}