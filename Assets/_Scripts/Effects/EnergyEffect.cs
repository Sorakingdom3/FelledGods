public class EnergyEffect : Effect
{
    public EnergyEffect(EffectAmount amount, int? duration = null, Enums.ModifierType level = Enums.ModifierType.Base, Enums.TargetType targetType = Enums.TargetType.Self, Enums.Stat statType = Enums.Stat.None) : base(amount, duration, level, targetType, statType)
    {
    }

    public override void Apply(Target source, Target target, Stats sourceStats)
    {
        var player = source as Player;
        player.AddEnergy(GetAmount());
    }
}
