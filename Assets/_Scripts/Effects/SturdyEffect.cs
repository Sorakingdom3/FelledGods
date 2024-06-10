public class SturdyEffect : ModifierEffect
{
    public SturdyEffect(EffectAmount amount, int? duration = null, Enums.ModifierType level = Enums.ModifierType.Base, Enums.TargetType targetType = Enums.TargetType.Self, Enums.Stat statType = Enums.Stat.None) : base(amount, duration, level, targetType, statType)
    {
    }

    public override void ApplyGrowth(Target target)
    {
        return;
    }

    public override ModifierEffect Copy(ModifierEffect effect)
    {
        EffectAmount amount = new EffectAmount();
        amount.BaseAmount = effect.Amount.BaseAmount;
        amount.EnchantedAmount = effect.Amount.EnchantedAmount;
        return new SturdyEffect(amount, Duration, Level, TargetType, StatType);
    }

    public override Enums.BuffType GetBuffType()
    {
        return Enums.BuffType.Sturdy;
    }

    public override int GetDisplayValue()
    {
        return GetAmount();
    }
    public override string GetDisplayName()
    {
        return base.GetDisplayName() + " <sprite=16>";
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
        Amount += effect.Amount;
    }
}
