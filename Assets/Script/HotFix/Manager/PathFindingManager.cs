using Pathfinding;
using UnityEngine;

/// <summary>
/// 寻路管理器
/// </summary>
public class PathFindingManager : IGameManager
{
    private AstarPath AstarPath;
    
    public void Init()
    {
    }

    public void Dispose()
    {
    }

    public void SetAStarPath(AstarPath astarPath)
    {
        AstarPath = astarPath;
        AstarPath.active.Scan();
    }
    
    public void FindPath(Seeker seeker, Transform target, OnPathDelegate callback)
    {
        seeker.StartPath(seeker.transform.position, target.position, callback);
    }
}
