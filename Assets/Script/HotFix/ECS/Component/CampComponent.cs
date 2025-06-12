using System;
using System.Collections.Generic;

/// <summary>
/// 阵营类型
/// </summary>
public enum CampType
{
    Role,
    Monster,
}

public class CampComponent : BaseComponent
{
    public override ComponentType Type => ComponentType.Camp;

    private CampType _campType;
    public CampType CampType => _campType;
    
    public CampComponent(CampType campType)
    {
        _campType = campType;
    }

    public List<CampType> GetEnemyCamp()
    {
        var campTypeList = new List<CampType>();
        switch (_campType)
        {
            case CampType.Role:
                campTypeList.Add(CampType.Monster);
                break;
            case CampType.Monster:
                campTypeList.Add(CampType.Role);
                break;
        }
        
        return campTypeList;
    }
}