using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// 消息管理器
/// </summary>
public class MessageManager : IGameManager
{
    private Dictionary<string, List<IMessageData>> MessageDict;

    public void Init()
    {
        MessageDict = new Dictionary<string, List<IMessageData>>();
    }

    public void Dispose()
    {
        MessageDict = null;
    }

    private bool TryGetMessageData<T>(List<IMessageData> messageDatas, out T resultData) where T : IMessageData
    {
        foreach (var data in messageDatas)
        {
            if (data is T asData)
            {
                resultData = asData;
                return true;
            }
        }

        resultData = default(T);
        return false;
    }

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="key"></param>
    /// <param name="dataAction"></param>
    public void Register(string key, UnityAction dataAction)
    {
        if (MessageDict.TryGetValue(key, out var previousActions))
        {
            if (TryGetMessageData<MessageData>(previousActions, out var messageData))
            {
                messageData.MessageEvents += dataAction;
            }
            else
            {
                previousActions.Add(new MessageData(dataAction));
            }
        }
        else
        {
            MessageDict.Add(key, new List<IMessageData>() { new MessageData(dataAction) });
        }
    }
    
    public void Register<T>(string key, UnityAction<T> dataAction)
    {
        if (MessageDict.TryGetValue(key, out var previousActions))
        {
            if (TryGetMessageData<MessageData<T>>(previousActions, out var messageData))
            {
                messageData.MessageEvents += dataAction;
            }
            else
            {
                previousActions.Add(new MessageData<T>(dataAction));
            }
        }
        else
        {
            MessageDict.Add(key, new List<IMessageData>() { new MessageData<T>(dataAction) });
        }
    }

    public void Register<T, D>(string key, UnityAction<T, D> dataAction)
    {
        if (MessageDict.TryGetValue(key, out var previousActions))
        {
            if (TryGetMessageData<MessageData<T, D>>(previousActions, out var messageData))
            {
                messageData.MessageEvents += dataAction;
            }
            else
            {
                previousActions.Add(new MessageData<T, D>(dataAction));
            }
        }
        else
        {
            MessageDict.Add(key, new List<IMessageData>() { new MessageData<T, D>(dataAction) });
        }
    }

    public void Register<T, D, U>(string key, UnityAction<T, D, U> dataAction)
    {
        if (MessageDict.TryGetValue(key, out var previousActions))
        {
            if (TryGetMessageData<MessageData<T, D, U>>(previousActions, out var messageData))
            {
                messageData.MessageEvents += dataAction;
            }
            else
            {
                previousActions.Add(new MessageData<T, D, U>(dataAction));
            }
        }
        else
        {
            MessageDict.Add(key, new List<IMessageData>() { new MessageData<T, D, U>(dataAction) });
        }
    }


    /// <summary>
    /// 注销
    /// </summary>
    /// <param name="key"></param>
    /// <param name="dataAction"></param>
    public void Remove(string key, UnityAction dataAction)
    {
        if (MessageDict.TryGetValue(key, out var previousActions))
        {
            if (TryGetMessageData<MessageData>(previousActions, out var messageData))
            {
                messageData.MessageEvents -= dataAction;
            }
        }
    }
    
    public void Remove<T>(string key, UnityAction<T> dataAction)
    {
        if (MessageDict.TryGetValue(key, out var previousActions))
        {
            if (TryGetMessageData<MessageData<T>>(previousActions, out var messageData))
            {
                messageData.MessageEvents -= dataAction;
            }
        }
    }

    public void Remove<T, U>(string key, UnityAction<T, U> dataAction)
    {
        if (MessageDict.TryGetValue(key, out var previousActions))
        {
            if (TryGetMessageData<MessageData<T, U>>(previousActions, out var messageData))
            {
                messageData.MessageEvents -= dataAction;
            }
        }
    }

    public void Remove<T, U, D>(string key, UnityAction<T, U, D> dataAction)
    {
        if (MessageDict.TryGetValue(key, out var previousActions))
        {
            if (TryGetMessageData<MessageData<T, U, D>>(previousActions, out var messageData))
            {
                messageData.MessageEvents -= dataAction;
            }
        }
    }

    /// <summary>
    /// 发送
    /// </summary>
    /// <param name="key"></param>
    public void Send(string key)
    {
        if (MessageDict.TryGetValue(key, out var previousActions))
        {
            if (TryGetMessageData<MessageData>(previousActions, out var messageData))
            {
                messageData.MessageEvents?.Invoke();
            }
        }
    }
    public void Send<T>(string key, T data)
    {
        if (MessageDict.TryGetValue(key, out var previousActions))
        {
            if (TryGetMessageData<MessageData<T>>(previousActions, out var messageData))
            {
                messageData.MessageEvents?.Invoke(data);
            }
        }
    }

    public void Send<T, U>(string key, T data1, U data2)
    {
        if (MessageDict.TryGetValue(key, out var previousActions))
        {
            if (TryGetMessageData<MessageData<T, U>>(previousActions, out var messageData))
            {
                messageData.MessageEvents?.Invoke(data1, data2);
            }
        }
    }

    public void Send<T, U, D>(string key, T data, U data2, D data3)
    {
        if (MessageDict.TryGetValue(key, out var previousActions))
        {
            if (TryGetMessageData<MessageData<T, U, D>>(previousActions, out var messageData))
            {
                messageData.MessageEvents?.Invoke(data, data2, data3);
            }
        }
    }
}