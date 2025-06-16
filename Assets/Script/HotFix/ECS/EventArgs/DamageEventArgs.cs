public enum DamageEventType
{
    AttackStart, // 攻击开始
    AttackHit, // 攻击命中
    AttackEnd, // 攻击结束
    DamageDealt, // 造成伤害
    DamageReceived // 受到伤害
}

public class DamageEventArgs : ComponentEventArgs
{
    public BaseSprite Attacker { get; set; }
    public BaseSprite Target { get; set; }
    public AttackData AttackData { get; set; }
    public float FinalDamage { get; set; }
    public DamageEventType EventType { get; set; }
}