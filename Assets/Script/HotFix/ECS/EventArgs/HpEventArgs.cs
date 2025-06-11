// HP事件类型
public enum HpEventType
{
    TakeDamage,
    Heal,
    Die,
    HpChanged,
}

/// <summary>
/// 事件参数
/// </summary>
public class HpEventArgs : ComponentEventArgs
{
    public float CurrentHp { get; set; }

    public float MaxHp { get; set; }

    public float DamageAmount { get; set; }

    public float HealAmount { get; set; }

    public HpEventType EventType { get; set; }
}