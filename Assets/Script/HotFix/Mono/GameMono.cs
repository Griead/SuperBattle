using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMono : BaseMono
{
    public RoleSprite roleSprite;
    public AstarPath astarPath;
    
    protected override void OnStart()
    {
        base.OnStart();
        
        Debug.Log("场景加载完毕" + " 准备进入");

        StartCoroutine(Load());
    }

    public IEnumerator Load()
    {
        // 初始化管理器
        yield return ManagerUtility.Initialize();

        Debug.Log("管理器初始化完成");

        GlobalManager.Instance.GetModel<PathFindingManager>().SetAStarPath(astarPath);
        GlobalManager.Instance.GetModel<GameGlobalManager>().SetRoleSprite(roleSprite);
        
        // string mapPrefabPath = ResourcesPathDefine.MainMap;
        // GlobalManager.Instance.GetModel<ResourcesManager>().LoadAssetAsync<GameObject>(mapPrefabPath, action: (asset) =>
        // {
        //     var go = Object.Instantiate(asset.AssetObject) as GameObject;
        //     go.transform.position = Vector3.zero;
        //
        //     var astarPath = go.GetComponent<AstarPath>();
        //     GlobalManager.Instance.GetModel<PathFindingManager>().SetAStarPath(astarPath);
        // });
        //
        // string rolePrefabPath = ResourcesPathDefine.RolePrefabPath;
        // GlobalManager.Instance.GetModel<ResourcesManager>().LoadAssetAsync<GameObject>(rolePrefabPath, action: (asset) =>
        // {
        //     var go = Object.Instantiate(asset.AssetObject) as GameObject;
        //     go.transform.position = Vector3.zero;
        //     
        //     var roleSprite = go.GetComponent<RoleSprite>();
        //     GlobalManager.Instance.GetModel<GameGlobalManager>().SetRoleSprite(roleSprite);
        // });
    }
}
