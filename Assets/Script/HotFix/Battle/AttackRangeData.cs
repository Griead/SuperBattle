using UnityEngine;

public enum AttackRangeType
{
    /// <summary> 圆形 </summary>
    Circle,

    /// <summary> 扇形 </summary>
    Sector,

    /// <summary> 矩形 </summary>
    Rectangle,

    /// <summary> 锥形 </summary>
    Cone,
}

public class AttackRangeData
{
    public AttackRangeType rangeType;

    /// <summary> 范围大小 </summary>
    public float range;

    /// <summary> 宽度（矩形） </summary>
    public float width;

    /// <summary> 高度（矩形） </summary>
    public float height;

    /// <summary> 角度（扇形/锥形） </summary>
    public float angle;

    /// <summary> 相对攻击点的偏移 </summary>
    public Vector2 offset;

    /// <summary> 旋转角度 </summary>
    public float rotation;

    /// <summary>
    /// 圆形范围
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public static AttackRangeData Circle(float radius, Vector2 offset = default)
    {
        return new AttackRangeData
        {
            rangeType = AttackRangeType.Circle,
            range = radius,
            offset = offset
        };
    }

    /// <summary>
    /// 扇形范围
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="angle"></param>
    /// <param name="rotation"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public static AttackRangeData Sector(float radius, float angle, float rotation = 0f, Vector2 offset = default)
    {
        return new AttackRangeData
        {
            rangeType = AttackRangeType.Sector,
            range = radius,
            angle = angle,
            rotation = rotation,
            offset = offset
        };
    }

    /// <summary>
    /// 矩形范围
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="rotation"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public static AttackRangeData Rectangle(float width, float height, float rotation = 0f, Vector2 offset = default)
    {
        return new AttackRangeData
        {
            rangeType = AttackRangeType.Rectangle,
            width = width,
            height = height,
            rotation = rotation,
            offset = offset
        };
    }

    /// <summary>
    /// 锥形范围
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="angle"></param>
    /// <param name="rotation"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public static AttackRangeData Cone(float radius, float angle, float rotation = 0f, Vector2 offset = default)
    {
        return new AttackRangeData
        {
            rangeType = AttackRangeType.Cone,
            range = radius,
            angle = angle,
            rotation = rotation,
            offset = offset
        };
    }
}