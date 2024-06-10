public class MalnourishedEffect : ModifierEffect
{
    public MalnourishedEffect(EffectAmount amount, int? duration = null, Enums.ModifierType level = Enums.ModifierType.Base, Enums.TargetType targetType = Enums.TargetType.Self, Enums.Stat statType = Enums.Stat.None) : base(amount, duration, level, targetType, statType)
    {
        Duration = amount.GetAmount(Level);
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
        return new FragileEffect(amount, Duration, Level, TargetType, StatType);
    }

    public override Enums.BuffType GetBuffType()
    {
        return Enums.BuffType.Malnourished;
    }

    public override int GetDisplayValue()
    {
        return Duration.Value;
    }
    public override string GetDisplayName()
    {
        return base.GetDisplayName() + " <sprite=4>";
    }
    public override void HandleDuration()
    {
        --Duration;
    }

    public override bool HasExpired()
    {
        return Duration <= 0;
    }

    public override void Stack(ModifierEffect effect)
    {
        Duration += effect.Duration;
    }
}
