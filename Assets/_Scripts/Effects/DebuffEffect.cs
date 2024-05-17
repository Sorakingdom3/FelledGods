public class DebuffEffect : Effect
{
    Buff debuff;

    public override void Apply(ITarget source, ITarget target, Enums.ModifierType type, Stats stats)
    {
        switch (TargetType)
        {
            case Enums.TargetType.SingleEnemy:
                //target.ApplyDebuff(debuff, type);
                break;
            case Enums.TargetType.MultipleEnemies:
                break;
        }
    }

}

