public class Relic : ILootable
{
    public int GetLootAmount()
    {
        return 1;
    }

    public Enums.LootType GetLootType()
    {
        return Enums.LootType.Relic;
    }
}