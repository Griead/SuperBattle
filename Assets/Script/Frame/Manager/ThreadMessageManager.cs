using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 网络请求数据
/// </summary>
public class NetRequestData
{
    public string Name;

    public object Content;
}

/// <summary>
/// 网络请求管理器
/// </summary>
public class ThreadMessageManager : BaseSingleton<ThreadMessageManager>, IGameManager, IUpdate
{
    public delegate void CustomDelegateHandle<in T>(T t);
    
    private Dictionary<string, List<CustomDelegateHandle<object>>> _handleCbDic;

    private List<NetRequestData> _handleList;

    public void Init()
    {
        _handleCbDic = new Dictionary<string, List<CustomDelegateHandle<object>>>();
        _handleList = new List<NetRequestData>();
    }
    public void Dispose()
    {
        _handleCbDic.Clear();
        _handleList.Clear();
    }

    public void Update(float _time)
    {
        //无请求
        if (_handleList == null || _handleList.Count <= 0)
            return;

        lock (_handleList)
        {
            int _handlecount = _handleList.Count;
            for (int i = 0; i < _handlecount; i++)
            {
                NetRequestData _data = _handleList[i];
                List<CustomDelegateHandle<object>> _handles = null;
                string _reqname = _data == null ? "NULL" : _data.Name;

                if (_data == null)
                    DebugUtility.LogError("Request Is Null! ", _handleList.Count);
                else if (_handleCbDic.ContainsKey(_reqname))
                    _handles = _handleCbDic[_reqname];

                _handleList.RemoveAt(i);
                i--;
                _handlecount--;

                if (_data == null)
                    continue;

                if (_handles == null)
                {
                    continue;
                }

                int _count = _handles.Count;
                for (int j = 0; j < _count; j++)
                {
                    _handles[j](_data.Content);
                }
            }
        }
    }

    /// <summary>
    /// 新增请求
    /// </summary>
    /// <param name="name"></param>
    /// <param name="_handle"></param>
    /// <param name="content"></param>
    public void AddHandle(string name, object content = null)
    {
        lock (_handleList)
        {
            _handleList.Add(new NetRequestData { Name = name, Content = content });
        }
    }
    
    /// <summary>
    /// 注册请求处理
    /// </summary>
    /// <param name="name"></param>
    /// <param name="handle"></param>
    public void RegisterHandleCb(string name, CustomDelegateHandle<object> handle)
    {
        if (!_handleCbDic.ContainsKey(name))
            _handleCbDic.Add(name, new List<CustomDelegateHandle<object>>());

        _handleCbDic[name].Add(handle);
    }
    /// <summary>
    /// 移除请求处理
    /// </summary>
    /// <param name="name"></param>
    /// <param name="handle"></param>
    public void RemoveHandleCb(string name, CustomDelegateHandle<object> handle)
    {
        if (!_handleCbDic.ContainsKey(name))
            return;

        _handleCbDic[name].Remove(handle);
        if (_handleCbDic[name].Count <= 0)
            _handleCbDic.Remove(name);
    }
}
