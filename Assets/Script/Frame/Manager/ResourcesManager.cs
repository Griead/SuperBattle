using System;
using System.Collections;
using System.Collections.Generic;
#if USING_UNITASK
using Task = Cysharp.Threading.Tasks.UniTask;
using ObjectTask = Cysharp.Threading.Tasks.UniTask<UnityEngine.Object>;
using GameObjectTask = Cysharp.Threading.Tasks.UniTask<UnityEngine.GameObject>;
using Cysharp.Threading.Tasks;
#else
using ObjectTask = System.Threading.Tasks.Task<UnityEngine.Object>;
using GameObjectTask = System.Threading.Tasks.Task<UnityEngine.GameObject>;
using System.Threading.Tasks;
#endif
using Feif.UIFramework;
using UnityEngine;
using YooAsset;

public partial class ResourcesManager : IGameManager
{
    /// <summary>
    /// 资源句柄字典
    /// </summary>
    private static Dictionary<Type, AssetHandle> AssetHandleDict;
    public void Init()
    {
        AssetHandleDict = new Dictionary<Type, AssetHandle>();
        Debug.Log("初始化资源管理器");
    }

    public void Dispose()
    {
        AssetHandleDict = null;
        YooAssetRelease();
    }

    /// <summary>
    /// 同步加载
    /// </summary>
    /// <param name="location"></param>
    /// <typeparam name="TObject"></typeparam>
    /// <returns></returns>
    public TObject LoadAsset<TObject>(string location) where TObject : UnityEngine.Object
    {
        var _handle = YooAssetPackage.LoadAssetSync<TObject>(location);
        return _handle.AssetObject as TObject;
    }

    /// <summary>
    /// 异步加载资源对象
    /// </summary>
    /// <typeparam name="TObject">资源类型</typeparam>
    /// <param name="location">资源的定位地址</param>
    public ObjectTask LoadAssetAsync<TObject>(string location, uint priority = 0, System.Action<AssetHandle> action = null) where TObject : UnityEngine.Object
    {
        var handle = YooAssetPackage.LoadAssetAsync<TObject>(location, priority);

        if (action != null)
            handle.Completed += action;

        return ToGameObjectTask<TObject>(handle);
    }

    /// <summary>
    /// 卸载资源列表
    /// </summary>
    /// <param name="locationList"></param>
    public void UnloadAssetList(List<string> locationList)
    {
        UnloadAssetList(locationList.ToArray());
    }
    
    /// <summary>
    /// 卸载资源列表
    /// </summary>
    /// <param name="locationList"></param>
    public void UnloadAssetList(params string[] locationList)
    {
        foreach (var location in locationList)
        {
            UnloadAsset(location);
        }
    }

    /// <summary>
    /// 卸载资源
    /// </summary>
    /// <param name="location"></param>
    public void UnloadAsset(string location)
    {
        YooAssetPackage.TryUnloadUnusedAsset(location);
    }

    private static ObjectTask ToGameObjectTask<TObject>(AssetHandle handle) where TObject : UnityEngine.Object
    {
#if USING_UNITASK
        return UniTask.Create(async () =>
        {
            await handle.ToUniTask();
            return handle.AssetObject;
        });
#else
        return handle.Task.ContinueWith(t =>
        {
            if (handle.Status == EOperationStatus.Succeed)
            {
                return handle.AssetObject;
            }
            else
            {
                Debug.LogError($"Error: {handle.LastError}");
                return null;
            }
        });
#endif
    }

    /// <summary>
    /// 资源请求事件
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static GameObjectTask OnUIAssetRequest(Type type)
    {
        AssetHandle handle = null;
        var _subFolderName = UIFrame.GetLayer(type);
        var location = $"Assets/AssetPackages/GameRes/UI/{_subFolderName.GetName()}/{type.Name}/{type.Name}";
        if (!AssetHandleDict.TryGetValue(type, out handle))
        {
            handle = YooAssetPackage.LoadAssetAsync<GameObject>(location);
        }

#if USING_UNITASK
        return UniTask.Create(async () =>
        {
            await handle.ToUniTask();
            return handle.AssetObject as GameObject;
        });
#else
        return handle.Task.ContinueWith(t =>
        {
            if (handle.Status == EOperationStatus.Succeed)
            {
                return handle.AssetObject as GameObject;
            }
            else
            {
                Debug.LogError($"Failed to load asset at location: {location}. Error: {handle.LastError}");
                return null;
            }
        });
#endif
    }
    
    /// <summary>
    /// 资源释放事件
    /// </summary>
    /// <param name="type"></param>
    public static void OnUIAssetRelease(Type type)
    {
        if(AssetHandleDict.ContainsKey(type))
        {
            AssetHandleDict[type].Release();
            AssetHandleDict.Remove(type);
        }
    }
}
