using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public List<StatDisplay> Displays = new List<StatDisplay>();

    public void UpdateDisplay(Stats stats)
    {
        foreach (var display in Displays)
        {
            if (display.m_Stat == Enums.Stat.Strength)
            {
                display.UpdateData(stats.Strength, stats.StrMod);
            }
            else if (display.m_Stat == Enums.Stat.Dexterity)
            {
                display.UpdateData(stats.Dexterity, stats.DexMod);
            }
            else if (display.m_Stat == Enums.Stat.Constitution)
            {
                display.UpdateData(stats.Constitution, stats.ConMod);
            }
            else if (display.m_Stat == Enums.Stat.Intelligence)
            {
                display.UpdateData(stats.Intelligence, stats.IntMod);
            }
            else if (display.m_Stat == Enums.Stat.Wisdom)
            {
                display.UpdateData(stats.Wisdom, stats.WisMod);
            }
            else if (display.m_Stat == Enums.Stat.Charisma)
            {
                display.UpdateData(stats.Charisma, stats.ChaMod);
            }
        }
    }
}
