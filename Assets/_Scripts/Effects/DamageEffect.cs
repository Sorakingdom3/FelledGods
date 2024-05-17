public class DamageEffect : Effect
{
    public override void Apply(ITarget source, ITarget target, Enums.ModifierType type, Stats stats)
    {
        switch (TargetType)
        {
            case Enums.TargetType.SingleEnemy:
                if (HasStatModifier) target.DealDamage(Amount.GetAmount(type) + stats.GetModifier(StatType));
                else target.DealDamage(Amount.GetAmount(type));
                break;

            case Enums.TargetType.MultipleEnemies:
                var enemies = BattleController.Instance.GetEnemies();
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (HasStatModifier) enemies[i].DealDamage(Amount.GetAmount(type) + stats.GetModifier(StatType));
                    else enemies[i].DealDamage(Amount.GetAmount(type));
                }
                break;
        }
    }
}
