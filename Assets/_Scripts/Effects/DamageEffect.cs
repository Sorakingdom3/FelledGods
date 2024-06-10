using UnityEngine;

public class DamageEffect : Effect
{

    public DamageEffect(EffectAmount amount, int? duration = null, Enums.ModifierType level = Enums.ModifierType.Base, Enums.TargetType targetType = Enums.TargetType.Self, Enums.Stat statType = Enums.Stat.None)
        : base(amount, duration, level, targetType, statType)
    {

    }

    public override void Apply(Target source, Target target, Stats stats)
    {
        int damage = (StatType != Enums.Stat.None) ? GetAmount() + stats.GetModifier(StatType) : GetAmount();
        if (source.Buffs.ContainsKey(Enums.BuffType.Strength))
        {
            damage += source.Buffs[Enums.BuffType.Strength].GetAmount();
        }

        if (source.Buffs.ContainsKey(Enums.BuffType.Weakness))
        {
            damage = Mathf.FloorToInt(damage * 0.75f);
        }

        switch (TargetType)
        {
            case Enums.TargetType.Self:

                source.RecieveDamage(damage);
                break;
            case Enums.TargetType.SingleEnemy:
                target.RecieveDamage(damage);
                break;
            case Enums.TargetType.MultipleEnemies:
                var enemies = BattleController.Instance.GetEnemies();
                for (int i = 0; i < enemies.Count;)
                {
                    bool dead = enemies[i].RecieveDamage(damage);
                    if (!dead || i >= enemies.Count)
                    {
                        ++i;
                    }

                }
                break;
        }
    }
}
