public class DefenseEffect : Effect
{
    public override void Apply(ITarget source, ITarget target, Enums.ModifierType type, Stats stats)
    {
        switch (TargetType)
        {
            case Enums.TargetType.Self:
                if (HasStatModifier) source.AddDefense(Amount.GetAmount(type) + stats.GetModifier(StatType));
                else source.AddDefense(Amount.GetAmount(type));
                break;
            case Enums.TargetType.SingleEnemy:
                if (HasStatModifier) target.AddDefense(Amount.GetAmount(type) + stats.GetModifier(StatType));
                else target.AddDefense(Amount.GetAmount(type));
                break;

            case Enums.TargetType.MultipleEnemies:
                foreach (ITarget hit in BattleController.Instance.GetEnemies())
                {
                    if (HasStatModifier) hit.AddDefense(Amount.GetAmount(type) + stats.GetModifier(StatType));
                    else hit.AddDefense(Amount.GetAmount(type));
                }
                break;

        }
    }
}
