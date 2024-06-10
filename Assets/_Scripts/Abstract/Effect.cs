using System;

[Serializable]
public abstract class Effect
{
    public EffectAmount Amount;
    public int? Duration;
    public Enums.ModifierType Level;
    public Enums.TargetType TargetType;
    public Enums.Stat StatType;

    protected Effect(EffectAmount amount, int? duration = null, Enums.ModifierType level = Enums.ModifierType.Base, Enums.TargetType targetType = Enums.TargetType.Self, Enums.Stat statType = Enums.Stat.None)
    {
        Amount = amount;
        Level = level;
        Duration = duration;
        TargetType = targetType;
        StatType = statType;
    }

    public abstract void Apply(Target source, Target target, Stats sourceStats);

    public virtual int GetAmount()
    {
        return Amount.GetAmount(Level);
    }
}

[Serializable]
public class EffectAmount
{
    public int BaseAmount;
    public int EnchantedAmount;
    public int GetAmount(Enums.ModifierType type)
    {
        switch (type)
        {
            case Enums.ModifierType.Enchanted:
                return EnchantedAmount;
            case Enums.ModifierType.Base:
            default:
                return BaseAmount;

        }
    }
    public static EffectAmount operator +(EffectAmount amount, EffectAmount other)
    {
        EffectAmount result = new EffectAmount();
        result.BaseAmount = amount.BaseAmount + other.BaseAmount;
        result.EnchantedAmount = amount.EnchantedAmount + other.EnchantedAmount;
        return result;
    }
}
