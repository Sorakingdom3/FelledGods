public class RageEffect : ModifierEffect
{

    public StrengthEffect Strength;
    public RageEffect(EffectAmount amount, int? duration = null, Enums.ModifierType level = Enums.ModifierType.Base, Enums.TargetType targetType = Enums.TargetType.Self, Enums.Stat statType = Enums.Stat.None) : base(amount, duration, level, targetType, statType)
    {
    }

    public override bool HasGrowth()
    {
        return true;
    }

    public override void ApplyGrowth(Target target)
    {
        Strength.Apply(target, target, null);
    }

    public override ModifierEffect Copy(ModifierEffect effect)
    {
        EffectAmount amount = new EffectAmount();
        amount.BaseAmount = effect.Amount.BaseAmount;
        amount.EnchantedAmount = effect.Amount.EnchantedAmount;

        var copy = new RageEffect(amount, Duration, Level, TargetType, StatType);
        var copied = effect as RageEffect;

        copy.Strength = copied.Strength.Copy(copied.Strength) as StrengthEffect;

        return copy;
    }

    public override Enums.BuffType GetBuffType()
    {
        return Enums.BuffType.Rage;
    }

    public override int GetDisplayValue()
    {
        return Strength.GetDisplayValue();
    }

    public override string GetDisplayName()
    {
        return base.GetDisplayName() + " <sprite=1>";
    }

    public override void HandleDuration()
    {
        return;
    }

    public override bool HasExpired()
    {
        return false;
    }

    public override void Stack(ModifierEffect effect)
    {
        var stacked = effect as RageEffect;
        Strength.Amount += stacked.Amount;
    }
}
