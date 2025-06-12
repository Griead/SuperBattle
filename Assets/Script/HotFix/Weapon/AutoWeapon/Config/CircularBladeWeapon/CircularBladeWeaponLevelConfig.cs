public class CircularBladeWeaponLevelConfig : IWeaponLevelConfig
{
    /// <summary>
    /// 累加伤害
    /// </summary>
    public float AccumulationDamage = 10f;
    /// <summary>
    /// 累加旋转速度
    /// </summary>
    public float AccumulationRotateSpeed = 5f;
    /// <summary>
    /// 累加刀刃数量
    /// </summary>
    public int AccumulationCount = 3;
    /// <summary>
    /// 累加持续时间
    /// </summary>
    public float AccumulationDuringTime = 5;

    /// <summary>
    /// 新冷却时间
    /// </summary>
    public float NewCoolDown = 1f;
    /// <summary>
    /// 新刀刃预制体路径
    /// </summary>
    public string NewBladePrefabPath = "Assets/AssetPackages/Prefab/Weapon/CircularBlade.prefab";
}