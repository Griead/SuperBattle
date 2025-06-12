using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 环绕刀
/// </summary>
public class CircularBlade : IAutoWeapon
{
    public AutoWeaponType Type => AutoWeaponType.CircularBlade;

    private BaseSprite Owner { get; set; }
    
    /// <summary> 等级配置列表 </summary>
    private List<CircularBladeWeaponLevelConfig> LevelConfigs;

    /// 刀刃实体
    private List<GameObject> InstanceList;

    public void Initialize(BaseSprite _owner, IWeaponLevelConfig config)
    {
        Owner = _owner;
        LevelConfigs = new List<CircularBladeWeaponLevelConfig>() { config as CircularBladeWeaponLevelConfig };
        CreateWeapon();
    }

    public void OnUpgrade(IWeaponLevelConfig config)
    {
        if(!LevelConfigs.Contains(config as CircularBladeWeaponLevelConfig))
            LevelConfigs.Add(config as CircularBladeWeaponLevelConfig);
        
        //刷新视图
        CreateWeapon();
    }

    /// <summary>
    /// 刷新视图
    /// </summary>
    private void CreateWeapon()
    {
        string newBladePrefabPath = "";
        float newCoolDown = 0f;
        int totalCount = 0;
        float totalSpeed = 0f;
        float totalDamage = 0f;

        int count = LevelConfigs?.Count ?? 0;
        for (int i = 0; i < count; i++)
        {
            totalCount += LevelConfigs[i].AccumulationCount;
            totalSpeed += LevelConfigs[i].AccumulationRotateSpeed;
            totalDamage += LevelConfigs[i].AccumulationDamage;

            if (!string.IsNullOrEmpty(LevelConfigs[i].NewBladePrefabPath))
                newBladePrefabPath = LevelConfigs[i].NewBladePrefabPath;
            
        }

        GlobalManager.Instance.GetModel<ResourcesManager>().LoadAssetAsync<GameObject>(newBladePrefabPath, action: (assetHandle) =>
        {
            var model = assetHandle.AssetObject as GameObject;
            for (int i = 0; i < totalCount; i++)
            {
                var go = Object.Instantiate(model);
                
            }
        });
    }

    public void OnUpdate(float deltaTime)
    {
    }
}