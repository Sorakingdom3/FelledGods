using Sirenix.OdinInspector;
using System;

[Serializable]
public class Stats
{
    [HorizontalGroup("Base")][OnValueChanged("SetModifiers")] public int Strength;
    [HorizontalGroup("Base")][OnValueChanged("SetModifiers")] public int Dexterity;
    [HorizontalGroup("Base")][OnValueChanged("SetModifiers")] public int Constitution;
    [HorizontalGroup("Base")][OnValueChanged("SetModifiers")] public int Intelligence;
    [HorizontalGroup("Base")][OnValueChanged("SetModifiers")] public int Wisdom;
    [HorizontalGroup("Base")][OnValueChanged("SetModifiers")] public int Charisma;

    [HorizontalGroup("Modifiers")][ReadOnly] public int StrMod;
    [HorizontalGroup("Modifiers")][ReadOnly] public int DexMod;
    [HorizontalGroup("Modifiers")][ReadOnly] public int ConMod;
    [HorizontalGroup("Modifiers")][ReadOnly] public int IntMod;
    [HorizontalGroup("Modifiers")][ReadOnly] public int WisMod;
    [HorizontalGroup("Modifiers")][ReadOnly] public int ChaMod;


    public Stats(int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma)
    {
        Strength = strength;
        Dexterity = dexterity;
        Constitution = constitution;
        Intelligence = intelligence;
        Wisdom = wisdom;
        Charisma = charisma;
        SetModifiers();
    }
    public void SetModifiers()
    {
        StrMod = (int)Math.Floor((Strength - 10) / 2.0);
        DexMod = (int)Math.Floor((Dexterity - 10) / 2.0);
        ConMod = (int)Math.Floor((Constitution - 10) / 2.0);
        IntMod = (int)Math.Floor((Intelligence - 10) / 2.0);
        WisMod = (int)Math.Floor((Wisdom - 10) / 2.0);
        ChaMod = (int)Math.Floor((Charisma - 10) / 2.0);
    }

    public void ApplyStatModifier(Enums.Stat stat, int amount)
    {
        switch (stat)
        {
            case Enums.Stat.Strength:
                Strength += amount;
                break;
            case Enums.Stat.Dexterity:
                Dexterity += amount;
                break;
            case Enums.Stat.Constitution:
                Constitution += amount;
                break;
            case Enums.Stat.Intelligence:
                Intelligence += amount;
                break;
            case Enums.Stat.Wisdom:
                Wisdom += amount;
                break;
            case Enums.Stat.Charisma:
                Charisma += amount;
                break;
        }
    }

    public int GetModifier(Enums.Stat modifierType)
    {
        switch (modifierType)
        {
            case Enums.Stat.Strength:
                return StrMod;
            case Enums.Stat.Dexterity:
                return DexMod;
            case Enums.Stat.Constitution:
                return ConMod;
            case Enums.Stat.Intelligence:
                return IntMod;
            case Enums.Stat.Wisdom:
                return WisMod;
            case Enums.Stat.Charisma:
                return ChaMod;
            default:
                return -1;
        }
    }

    public int GetBase(Enums.Stat statType)
    {
        switch (statType)
        {
            case Enums.Stat.Strength:
                return Strength;
            case Enums.Stat.Dexterity:
                return Dexterity;
            case Enums.Stat.Constitution:
                return Constitution;
            case Enums.Stat.Intelligence:
                return Intelligence;
            case Enums.Stat.Wisdom:
                return Wisdom;
            case Enums.Stat.Charisma:
                return Charisma;
            default:
                return -1;
        }
    }
}