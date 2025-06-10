using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseSprite : BaseMono
{
    private Dictionary<ComponentType, IComponent> _components = new Dictionary<ComponentType, IComponent>();

    private Dictionary<ComponentType, List<Action<ComponentEventArgs>>> _moduleCallbacks = new Dictionary<ComponentType, List<Action<ComponentEventArgs>>>();

    #region 模块管理

    public BaseSprite AddEComponent<T>(ComponentType type, Action<ComponentEventArgs> callback = null) where T : IComponent, new()
    {
        if (!_components.ContainsKey(type))
        {
            var component = new T();
            _components[type] = component;
            component.Initialize(this);

            if (callback != null)
            {
                RegisterEComponentCallback(type, callback);
            }
        }

        return this;
    }
    
    public BaseSprite AddEComponent(IComponent component, Action<ComponentEventArgs> callback = null)
    {
        if (!_components.ContainsKey(component.Type))
        {
            _components[component.Type] = component;
            component.Initialize(this);
            
            if (callback != null)
            {
                RegisterEComponentCallback(component.Type, callback);
            }
        }
        return this;
    }

    public void RemoveEComponent(ComponentType type)
    {
        if (_components.TryGetValue(type, out var component))
        {
            component.OnRelease();
            _components.Remove(type);
            _moduleCallbacks.Remove(type);
        }
    }

    public T GetEComponent<T>(ComponentType type) where T : class, IComponent
    {
        return _components.TryGetValue(type, out var component) ? component as T : null;
    }

    public bool HasEComponent(ComponentType type)
    {
        return _components.ContainsKey(type);
    }

    #endregion

    #region 事件系统
    
    public void RegisterEComponentCallback(ComponentType type, Action<ComponentEventArgs> callback)
    {
        if (!_moduleCallbacks.ContainsKey(type))
        {
            _moduleCallbacks[type] = new List<Action<ComponentEventArgs>>();
        }

        _moduleCallbacks[type].Add(callback);
    }

    public void TriggerEComponentEvent(ComponentType type, ComponentEventArgs args)
    {
        args.Sender = this;

        if (_moduleCallbacks.TryGetValue(type, out var callbacks))
        {
            foreach (var callback in callbacks)
            {
                callback?.Invoke(args);
            }
        }
    }

    #endregion

    #region Unity生命周期

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        foreach (var component in _components.Values)
        {
            component.Update(deltaTime);
        }
    }

    protected override void OnFixedUpdate(float fixedDeltaTime)
    {
        base.OnFixedUpdate(fixedDeltaTime);

        foreach (var component in _components.Values)
        {
            component.FixedUpdate(fixedDeltaTime);
        }
    }

    protected override void OnRelease()
    {
        foreach (var component in _components.Values)
        {
            component.OnRelease();
        }
        
        _components.Clear();
        _moduleCallbacks.Clear();
        
        base.OnRelease();
    }

    #endregion
}