/// <summary>
/// 宏定义管理器
/// </summary>
public class MacroDefinitionManager : IGameManager
{
    /// <summary>构建版本 </summary>
    public BuildVersion Version;

    /// <summary> 是否是脱机 </summary>
    public bool IsOffline;

    /// <summary> 资源加载启动模式 </summary>
    public YooAsset.EPlayMode AssetPlayMode;

    public void Init()
    {
#if UNITY_EDITOR
        Version = BuildVersion.Editor;
#elif UNITY_ANDROID
        Version = BuildVersion.Android;
#elif UNITY_IOS
        Version = BuildVersion.IOS;
#elif UNITY_WEBGL
        Version = BuildVersion.WeiXinMinGame;
#else
        Version = BuildVersion.Editor;
#endif
        DebugUtility.Log("当前的运行版本为: ", Version);

#if IsOffline
        IsOffline = true;
#else
        IsOffline = false;
#endif
        DebugUtility.Log("当前的联机模式:", !IsOffline);
    }

    public void Dispose()
    {
        
    }
}