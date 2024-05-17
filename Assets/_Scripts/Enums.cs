using System;

[Serializable]
public static class Enums
{
    [Serializable]
    public enum Class
    {
        Barbarian = 0,
        Wizard = 1,
        Cleric = 2,
        Druid = 3,
        Artificer = 4
    }
    [Serializable]
    public enum Stat
    {
        Strength = 0,
        Dexterity = 1,
        Constitution = 2,
        Intelligence = 3,
        Wisdom = 4,
        Charisma = 5
    }
    [Serializable]
    public enum Difficulties
    {
        No_Modifiers = 0,
        MurderHobbo = 1,
        Mercenary = 2,
        Skilled_Mercenary = 3,
        Adventurer = 4,
        Folk_Hero = 5,
        Royal_Knight = 6,
        Demi_God = 7,
        God = 8,
        God_Slayer = 9,
        Infinite_Divinity = 10
    }
    [Serializable]
    public enum NodeType
    {
        Undefined = -1,
        Start = 0,
        Shop = 1,
        Event = 2,
        Fight = 3,
        Inn = 4,
        Chest = 5,
        Elite = 6,
        Boss = 7
    }
    [Serializable]
    public enum CardType
    {
        Weapon = 0,
        Spell = 1,
        Defense = 2,
        Heal = 3,
        Consumable = 4, // Objects / Buff / Debuff
    }
    [Serializable]
    public enum RarityType
    {
        Common = 0,
        Rare = 1,
        Legendary = 2
    }
    [Serializable]
    public enum ModifierType
    {
        Base = 0,
        Enchanted = 1,
    }
    [Serializable]
    public enum ElementType
    {
        None = 0,
        Fire = 1,
        Ice = 2,
        Thuder = 3,
        Lightning = 4,
        Death = 5,
        Holy = 6

    }
    [Serializable]
    public enum TargetType
    {
        SingleEnemy = 0,
        MultipleEnemies = 1,
        Self = 3,
    }
    [Serializable]
    public enum AllowedTarget
    {
        Self = 0,
        Ally = 1,
        Enemy = 2
    }
    [Serializable]
    public enum BuffType
    {
        //Buffs
        Stat_Improvement = 0,
        Stat_Decrease = 1,
        DamageIncrease = 2,
        DamageDecrease = 3,
        //Debuff
        Expirates = 10 // Card goes to exhausted pile

    }
    [Serializable]
    public enum EffectDuration
    {
        Instant = 0,
        Round = 1,
        Multiple_Rounds = 2,
        Combat = 3,
        Permanent = 4
    }
    [Serializable]
    public enum LootType
    {
        Gold = 0,
        Relic = 1,
        Cards = 2,
        Potion = 3
    }
    [Serializable]
    public enum CardListMode
    {
        Collection = 0,
        Removal = 1,
        Enchant = 2
    }

    public enum EnemyType
    {
        Common = 0,
        Elite = 1,
        Boss = 2
    }
}