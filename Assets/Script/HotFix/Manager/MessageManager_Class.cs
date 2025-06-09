using UnityEngine.Events;

/// <summary>
/// 消息数据接口
/// </summary>
public interface IMessageData
{
}

public class MessageData : IMessageData
{
    public UnityAction MessageEvents;

    public MessageData(UnityAction action)
    {
        MessageEvents += action;
    }
}

public class MessageData<T> : IMessageData
{
    public UnityAction<T> MessageEvents;

    public MessageData(UnityAction<T> action)
    {
        MessageEvents += action;
    }
}

public class MessageData<T, D> : IMessageData
{
    public UnityAction<T, D> MessageEvents;

    public MessageData(UnityAction<T, D> action)
    {
        MessageEvents += action;
    }
}

public class MessageData<T, D, U> : IMessageData
{
    public UnityAction<T, D, U> MessageEvents;

    public MessageData(UnityAction<T, D, U> action)
    {
        MessageEvents += action;
    }
}