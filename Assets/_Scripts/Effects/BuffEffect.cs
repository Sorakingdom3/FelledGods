public class BuffEffect : Effect
{
    public Buff Buff;

    public override void Apply(ITarget source, ITarget target, Enums.ModifierType type, Stats stats)
    {
        switch (TargetType)
        {
            case Enums.TargetType.Self:

                break;
            case Enums.TargetType.SingleEnemy:
                //target.ApplyBuff(Buff, type);
                break;
            case Enums.TargetType.MultipleEnemies:
                //foreach (ITarget hit in info.GetAllAllies(target))
                //{
                //    hit.ApplyBuff(BuffType, Amount, BuffDuration);
                //}
                break;
                //// Get all the target allies
                //var objectives = info.GetAllAllies(target);
                //List<ITarget> adjacentToTarget = new List<ITarget>();
                //// choose adjacent targets
                //for (int i = 0; i < objectives.Count; i++)
                //{
                //    if (objectives[i] == target)
                //    {
                //        if (i > 0)
                //        {
                //            adjacentToTarget.Add(objectives[i - 1]);
                //        }
                //        if (i < objectives.Count - 1)
                //        {
                //            adjacentToTarget.Add(objectives[i + 1]);
                //        }
                //    }
                //}
                //// Apply Effect to adjacent targets.
                //foreach (ITarget hit in adjacentToTarget)
                //{
                //    hit.ApplyBuff(BuffType, Amount, BuffDuration);
                //} 
        }
    }

}
