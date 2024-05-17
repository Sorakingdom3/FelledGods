public class HealEffect : Effect
{
    public override void Apply(ITarget source, ITarget target, Enums.ModifierType type, Stats stats)
    {
        switch (TargetType)
        {
            case Enums.TargetType.Self:
            case Enums.TargetType.SingleEnemy:
                if (HasStatModifier) target.Heal(Amount.GetAmount(type) + stats.GetModifier(StatType));
                else target.Heal(Amount.GetAmount(type));
                break;

            case Enums.TargetType.MultipleEnemies:
                foreach (ITarget hit in BattleController.Instance.GetEnemies())
                {
                    if (HasStatModifier) hit.Heal(Amount.GetAmount(type) + stats.GetModifier(StatType));
                    else hit.Heal(Amount.GetAmount(type));
                }
                break;


        }
    }

}
