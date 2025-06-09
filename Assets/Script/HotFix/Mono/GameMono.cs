using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMono : BaseMono
{
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

        GlobalManager.Instance.GetModel<PathFindingManager>();
    }
}
