public class DefenseEffect : Effect
{
    public DefenseEffect(EffectAmount amount, int? duration = null, Enums.ModifierType level = Enums.ModifierType.Base, Enums.TargetType targetType = Enums.TargetType.Self, Enums.Stat statType = Enums.Stat.None)
        : base(amount, duration, level, targetType, statType)
    {
    }

    public override void Apply(Target source, Target target, Stats stats)
    {
        var shield = (StatType != Enums.Stat.None) ? GetAmount() + stats.GetModifier(StatType) : GetAmount();

        switch (TargetType)
        {
            case Enums.TargetType.Self:
                source.AddDefense(shield);
                break;
            case Enums.TargetType.SingleEnemy:
                target.AddDefense(shield);
                break;
            case Enums.TargetType.MultipleEnemies:
                foreach (Target enemy in BattleController.Instance.GetEnemies())
                {
                    enemy.AddDefense(shield);
                }
                break;
        }
    }
}
