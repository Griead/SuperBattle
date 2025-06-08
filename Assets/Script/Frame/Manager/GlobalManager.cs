using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : BaseSingleton<GlobalManager>, IGameManager,  IUpdate
{
    private Dictionary<Type,IGameManager> _managerList;

    public void Init()
    {
        _managerList = new Dictionary<Type, IGameManager>();
    }

    public void Dispose()
    {
        RemoveAllModule();
        _managerList = null;
    }
    
    public T RegisterModule<T>() where T : IGameManager, new()
    {
        T manager = new T();
        
        manager.Init();
        _managerList.Add(typeof(T), manager);

        return manager;
    }
    
    /// <summary>
    /// 获取管理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetModel<T>() where T : IGameManager, new()
    {
        if (_managerList.TryGetValue(typeof(T), out var value))
        {
            return (T)value;
        }

        DebugUtility.LogError("未找到管理器", typeof(T));
        return default;
    }

    private void RemoveAllModule()
    {
        var typeList = new List<Type>(_managerList.Keys);

        for (int i = typeList.Count - 1; i >= 0; i--)
        {
            var type = typeList[i];
            RemoveModule(type);
        }
    }

    private void RemoveModule(Type _type)
    {
        if (!_managerList.TryGetValue(_type, out var value))
            return;
        
        value.Dispose();
        _managerList.Remove(_type);
    }

    public void Update(float _time)
    {
        if(_managerList == null)
            return;
        
        foreach (var gameManager in _managerList.Values)
        {
            if (gameManager is IUpdate update)
            {
                update.Update(_time);
            }
        }
    }
}
