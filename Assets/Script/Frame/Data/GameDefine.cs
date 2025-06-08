using System.Collections;
using System.Collections.Generic;using System.Runtime.InteropServices;
using UnityEngine;

public struct GameDefine
{
    /// <summary>
    /// ui脚本目录
    /// </summary>
    public const string UIScriptFolder = "Assets/Scripts/Game/UI";

    /// <summary>
    /// UI表目录
    /// </summary>
    public const string UITableFolder = "Assets/AssetPackages/GameRes/DataTable/AutoTable";
    
    /// <summary>
    /// 英文库
    /// </summary>
    public const string EnglishLab = "EN";
    
    /// <summary>
    /// 中文库简体
    /// </summary>
    public const string ChineseSLab = "CHS";
    
    /// <summary>
    /// 中文库繁体
    /// </summary>
    public const string ChineseTLab = "CHT";
    
    /// <summary>
    /// 本地的设置路径
    /// </summary>
    public const string MyCustomSettingsPath = "Assets/Editor/MyCustomSettings.asset";

    /// <summary>
    /// 身高
    /// </summary>
    public const int MaxHeight = 250;
    public const int MinHeight = 130;
    
    /// <summary>
    /// 体重
    /// </summary>
    public const int MaxWeight = 300;
    public const int MinWeight = 70;

    // 预设图片
    public const string PromptHairColorIcon = "AssetPackages/GameRes/Picture/CommonBg/HairColorIcon.png";
    public const string PromptHairStyleIcon = "AssetPackages/GameRes/Picture/CommonBg/HairStyleIcon.png";
    public const string PromptEyesIcon = "AssetPackages/GameRes/Picture/CommonBg/EyesIcon.png";
    // public const string PromptNoseIcon = "AssetPackages/GameRes/Picture/CommonBg/HairColorIcon.png";
    public const string PromptFacialFeaturesIcon = "AssetPackages/GameRes/Picture/CommonBg/FacialFeaturesIcon.png";
    public const string PromptBodyIcon = "AssetPackages/GameRes/Picture/CommonBg/BodyIcon.png";

    public const string GrayHex = "#545454";

    /// <summary>
    /// 关系最大字符数
    /// </summary>
    public const int RelationshipMaxCharCount = 20;

    /// <summary>
    /// 名字最大字符数
    /// </summary>
    public const int NameMaxCharCount = 20;

    /// <summary>
    /// 专业最大字符数
    /// </summary>
    public const int MajorFieldMaxCharCount = 20;

    /// <summary>
    /// 社会关系
    /// </summary>
    public const int SocialRelationshipsMaxCharCount = 500;
    
    /// <summary>
    /// 生活经历 
    /// </summary>
    public const int LifeExperienceMaxCharCount = 500;
    
    /// <summary>
    /// 交互Floor名称
    /// </summary>
    public const string TileDefaultCommonFloorName = "Art";
    /// <summary>
    /// 交互Tilemap名称
    /// </summary>
    public const string TileDefaultInteractionName = "Object Interaction Blocks";
    /// <summary>
    /// 扇区中心Tilemap名称
    /// </summary>
    public const string TileDefaultSectorCenterName = "Sector Center";
    /// <summary>
    /// 碰撞Tilemap名称
    /// </summary>
    public const string TileDefaultCollisionsName = "Collisions";
    /// <summary>
    /// 扇区Tilemap名称
    /// </summary>
    public const string TileDefaultArenaBlocksName = "Arena Blocks";
    /// <summary>
    /// 扇区Tilemap名称
    /// </summary>
    public const string TileDefaultSectorBlocksName = "Sector Blocks";
}

public struct ResourcesPathDefine
{
    /// <summary>
    /// 家园Tilemap路径
    /// </summary>
    public const string HomeTilePath = "Assets/AssetPackages/GameRes/Tiled/Map_Bg_Home.tmx";
    
    /// <summary>
    /// 城镇Tilemap路径
    /// </summary>
    public const string TownTilePath = "Assets/AssetPackages/GameRes/Tiled/Map_Bg_Town.tmx";
    
    /// <summary>
    /// tile相机
    /// </summary>
    public const string TileCameraPath = "Assets/AssetPackages/GameRes/Tiled/TileCamera/Camera.prefab";
    
    /// <summary>
    /// tile角色
    /// </summary>
    public const string TileRolePath = "Assets/AssetPackages/GameRes/Tiled/TileRole/Role.prefab";
}
