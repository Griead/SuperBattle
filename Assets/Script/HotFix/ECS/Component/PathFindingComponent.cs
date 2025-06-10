using Pathfinding;
using UnityEngine;

public class PathFindingComponent : BaseComponent
{
    public override ComponentType Type => ComponentType.PathFinding;

    public Seeker Seeker { get; set; }
    public float Speed { get; set; } = 20f;
    // 寻路目标
    public Transform Target { get; set; }


    private Path Path { get; set; }
    private int CurrentWaypoint { get; set; }

    private bool IsCalculatingPath = false;
    private float PathCalculateTimer = 0f;
    private float PathCalculateInterval = 0.5f;

    private float PathFindingTimer = 0;
    private float PathFindingInterval = 0f;

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if (Target is null || Seeker is null)
            return;

        if (!IsCalculatingPath)
        {
            PathCalculateTimer += deltaTime;
            if (PathCalculateTimer >= PathCalculateInterval)
            {
                PathCalculateTimer -= PathCalculateInterval;

                IsCalculatingPath = true;
                GlobalManager.Instance.GetModel<PathFindingManager>().FindPath(Seeker, Target, OnPathComplete);
            }
        }

        PathFindingTimer += deltaTime;
        if (PathFindingTimer >= PathFindingInterval)
        {
            PathFindingTimer -= PathFindingInterval;

            if (Path != null && !Path.error)
            {
                // 移动到下一个路径点
                Vector3 direction = (Path.vectorPath[CurrentWaypoint] - Owner.transform.position).normalized;
                Owner.transform.position += direction * (Speed * Time.deltaTime);

                // 检查是否到达当前路径点
                if (Vector3.Distance(Owner.transform.position, Path.vectorPath[CurrentWaypoint]) < 0.1f)
                {
                    if (CurrentWaypoint < Path.vectorPath.Count - 1)
                        CurrentWaypoint++;
                }
            }
        }
    }

    private void OnPathComplete(Path path)
    {
        IsCalculatingPath = false;
        
        // 路径计算是否被中断
        if (path.error)
            return;

        Path = path;
        CurrentWaypoint = 0;
    }

    public override void OnRelease()
    {
        base.OnRelease();

        Seeker.CancelCurrentPathRequest();
    }
}