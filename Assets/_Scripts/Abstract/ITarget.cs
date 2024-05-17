public interface ITarget
{
    /// <summary>
    /// Character increases defense
    /// </summary> 
    void AddDefense(int amount);
    /// <summary>
    /// Character receives Damage
    /// </summary> 
    void DealDamage(int amount);
    /// <summary>
    /// Character heals
    /// </summary> 
    void Heal(int amount);
    /// <summary>
    /// Character recieves a Buff
    /// </summary> 
    //void ApplyBuff(Buff buff, Enums.ModifierType type);
    /// <summary>
    /// Character recieves a Debuff
    /// </summary> 
    //void ApplyDebuff(Buff buff, Enums.ModifierType type);
}