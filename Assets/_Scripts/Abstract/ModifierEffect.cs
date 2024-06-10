public abstract class ModifierEffect : Effect
{

    protected ModifierEffect(EffectAmount amount, int? duration = null, Enums.ModifierType level = Enums.ModifierType.Base, Enums.TargetType targetType = Enums.TargetType.Self, Enums.Stat statType = Enums.Stat.None)
        : base(amount, duration, level, targetType, statType)
    {

    }

    public virtual bool HasGrowth()
    {
        return false;
    }

    public abstract void HandleDuration();

    public abstract bool HasExpired();

    public abstract Enums.BuffType GetBuffType();
    public abstract ModifierEffect Copy(ModifierEffect effect);

    public abstract void Stack(ModifierEffect effect);
    public abstract void ApplyGrowth(Target target);
    public override void Apply(Target source, Target target, Stats sourceStats)
    {
        switch (TargetType)
        {
            case Enums.TargetType.Self:
                source.ApplyModification(Copy(this));
                break;
            case Enums.TargetType.SingleEnemy:

                target.ApplyModification(Copy(this));
                break;
            case Enums.TargetType.MultipleEnemies:
                var enemies = BattleController.Instance.GetEnemies();
                foreach (var enemy in enemies)
                {
                    enemy.ApplyModification(Copy(this));
                }
                break;
        }
    }
    public virtual string GetDisplayName()
    {
        string name = GetType().Name;
        return name.Replace("Effect", string.Empty);
    }
    public abstract int GetDisplayValue();
}