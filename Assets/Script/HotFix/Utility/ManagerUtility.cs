using System;
using System.Collections;
using System.Collections.Generic;

public class ManagerUtility
{
    /// <summary>
    /// 加载的管理器数量
    /// </summary>
    public static readonly List<Type> ManagerList = new()
    {
        typeof(MacroDefinitionManager),
        typeof(PathFindingManager),
    };
    
    public static IEnumerator Initialize()
    {
        // 全局管理器初始化
        GlobalManager.Instance.Init();
        
        // 宏定义
        GlobalManager.Instance.RegisterModule<MacroDefinitionManager>();
        yield return null;

        // 寻路
        GlobalManager.Instance.RegisterModule<PathFindingManager>();
        yield return null;
        
        // 寻路
        GlobalManager.Instance.RegisterModule<ThreadMessageManager>();
        yield return null;
        
        // 寻路
        GlobalManager.Instance.RegisterModule<MessageManager>();
        yield return null;
        
        // 寻路
        GlobalManager.Instance.RegisterModule<ResourcesManager>();
        yield return null;
        
        
    }
}