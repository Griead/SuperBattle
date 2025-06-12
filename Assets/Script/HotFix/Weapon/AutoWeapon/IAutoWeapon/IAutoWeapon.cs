public enum AutoWeaponType
{
    CircularBlade
}

/// <summary>
/// 武器接口
/// </summary>
public interface IAutoWeapon
{
    AutoWeaponType Type { get; }
    
    /// <summary>
    /// 初始化
    /// </summary>
    void Initialize(BaseSprite _owner, IWeaponLevelConfig config);

    /// <summary>
    /// 升级
    /// </summary>
    void OnUpgrade(IWeaponLevelConfig config);
}