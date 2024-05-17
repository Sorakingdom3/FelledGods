using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI StatName;
    [SerializeField] private TextMeshProUGUI StatModifier;
    [SerializeField] private TextMeshProUGUI StatBase;
    [SerializeField] private Image DisplayBackgroud;
    public Enums.Stat m_Stat;

    public void Setup(Enums.Stat stat, int total, int modifier)
    {
        m_Stat = stat;
        StatName.text = ConvertToMod(stat);
        StatModifier.text = (modifier > 0) ? "+" + modifier : modifier.ToString();
        StatBase.text = total.ToString();
    }

    private string ConvertToMod(Enums.Stat stat)
    {
        switch (stat)
        {
            default:
            case Enums.Stat.Strength: return "STR";
            case Enums.Stat.Dexterity: return "DEX";
            case Enums.Stat.Constitution: return "CON";
            case Enums.Stat.Intelligence: return "INT";
            case Enums.Stat.Wisdom: return "WIS";
            case Enums.Stat.Charisma: return "CHA";
        }
    }

    public void UpdateData(int total, int modifier)
    {
        StatModifier.text = modifier.ToString();
        StatBase.text = total.ToString();
    }
}
