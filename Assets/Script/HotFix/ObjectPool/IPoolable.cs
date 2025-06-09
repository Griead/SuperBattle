/// <summary>
/// 对象池接口
/// </summary>
public interface IPoolable
{
    /// 从池中取出
    void OnSpawn();
    
    /// 放入池中
    void OnDespawn();
}