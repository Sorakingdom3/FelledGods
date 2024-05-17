using Sirenix.OdinInspector;
using System;

[Serializable]
public abstract class Effect
{
    public EffectAmount Amount;
    public Enums.TargetType TargetType;
    public bool HasStatModifier;
    [ShowIf("HasStatModifier")]
    public Enums.Stat StatType;

    public abstract void Apply(ITarget source, ITarget target, Enums.ModifierType level, Stats stats);
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
}
