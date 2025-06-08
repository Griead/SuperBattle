using System;
using System.Collections.Generic;

public class World
{
    private int nextEntityId = 0;
    private Dictionary<int, Dictionary<Type, IComponent>> entitiesComponents = new();
    
    public Entity CreateEntity()
    {
        var entity = new Entity(nextEntityId++);
        entitiesComponents.Add(entity.Id, new Dictionary<Type, IComponent>());
        return entity;
    }
    
    public void AddComponent<T>(Entity entity, T component) where T : IComponent
    {
        entitiesComponents[entity.Id][typeof(T)] = component;
    }
    
    public T GetComponent<T>(Entity entity) where T : IComponent
    {
        return (T)entitiesComponents[entity.Id][typeof(T)];
    }
    
    public bool HasComponent<T>(Entity entity) where T : IComponent
    {
        return entitiesComponents[entity.Id].ContainsKey(typeof(T));
    }

    public IEnumerable<Entity> GetEntitiesWithComponent<T1, T2>() where T1 : IComponent where T2 : IComponent
    {
        foreach (var pair in entitiesComponents)
        {
            if(pair.Value.ContainsKey(typeof(T1)) && pair.Value.ContainsKey(typeof(T2)))
                yield return new Entity(pair.Key);
        }
    }
}