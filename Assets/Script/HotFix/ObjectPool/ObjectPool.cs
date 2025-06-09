using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

// 溢出处理策略枚举
public enum PoolOverflowStrategy
{
    CreateNew,
    ReturnNull,
    ReplaceOldest,
}

public class ObjectPool<T> where T : BaseSprite, IPoolable
{
    private Queue<T> availableObjects = new Queue<T>();
    private HashSet<T> activeObjects = new HashSet<T>();
    private T prefab;
    private Transform parent;
    private int initialSize;
    private int maxPoolSize;
    private PoolOverflowStrategy overflowStrategy;

    public ObjectPool(T prefab, int initialSize = 10, PoolOverflowStrategy strategy = PoolOverflowStrategy.CreateNew, int maxPoolSize = 50, Transform parent = null)
    {
        this.prefab = prefab;
        this.initialSize = initialSize;
        this.overflowStrategy = strategy;
        this.maxPoolSize = maxPoolSize;
        this.parent = parent;
    }

    /// <summary>
    /// 初始化池
    /// </summary>
    private void InitialPool()
    {
        for (int i = 0; i < initialSize; i++)
        {
            T obj = CreateNewObject();
            obj.gameObject.SetActive(false);
        }
    }

    private T CreateNewObject()
    {
        T obj = Object.Instantiate(prefab, parent);
        return obj;
    }

    public T Spawn(Vector3 position, Quaternion rotation)
    {
        T obj = null;
        
        if (availableObjects.Count > 0)
        {
            // 优先从可用对象中获取
            obj = availableObjects.Dequeue();
        }
        else
        {
            // 处理溢出情况
            obj = HandleOverflow();
        }

        if (obj != null)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.gameObject.SetActive(true);
            obj.OnSpawn();
            activeObjects.Add(obj);
        }

        return obj;
    }

    private T HandleOverflow()
    {
        switch (overflowStrategy)
        {
            case PoolOverflowStrategy.CreateNew:
                {
                    return CreateNewObject();
                }
            case PoolOverflowStrategy.ReturnNull:
                {
                    Debug.Log("返回Null");
                    return null;
                }
            case PoolOverflowStrategy.ReplaceOldest:
                {
                    return ReplaceOldestObject();
                }
            default:
                return CreateNewObject();
        }
    }

    private T ReplaceOldestObject()
    {
        // 选择一个激活的对象
        if(activeObjects.Count > 0)
        {
            var enumerator = activeObjects.GetEnumerator();
            enumerator.MoveNext();
            T oldestObj = enumerator.Current;
            ForceRecycle(oldestObj);
            return oldestObj;
        }

        return CreateNewObject();
    }

    private void ForceRecycle(T obj)
    {
        if (obj != null && activeObjects.Contains(obj))
        {
            obj.OnDespawn();
            activeObjects.Remove(obj);
        }
    }

    public void Despawn(T obj)
    {
        if(obj == null || !activeObjects.Contains(obj))
            return;
        
        obj.OnDespawn();
        obj.gameObject.SetActive(false);
        activeObjects.Remove(obj);

        if (availableObjects.Count < maxPoolSize)
        {
            availableObjects.Enqueue(obj);
        }
        else
        {
            // 池子已经满了 销毁对象
            Object.Destroy(obj.gameObject);
        }
    }
}