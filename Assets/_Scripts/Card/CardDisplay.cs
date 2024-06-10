using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] public CardData _cardData;
    // UI Elements
    public TextMeshProUGUI m_DisplayName;
    public TextMeshProUGUI m_DisplayCost;
    public TextMeshProUGUI m_DisplayDescription;
    public Image m_DisplaySprite;
    public Image m_DisplayBorder;

    // Modifiers 
    public void Setup(CardData cardData)
    {
        _cardData = cardData;
        m_DisplayName.text = _cardData.Name;
        m_DisplayCost.text = _cardData.Cost.ToString();
        m_DisplayDescription.text = GetParsedDescription();
        m_DisplayBorder.sprite = _cardData.BorderSprite;
        m_DisplaySprite.sprite = _cardData.CardSprite;
    }

    private string GetParsedDescription()
    {
        string description = string.Empty;
        foreach (var effect in _cardData.Effects)
        {
            string effectDesc = string.Empty;
            string amount = (effect.GetAmount() > 0) ? effect.GetAmount().ToString() : string.Empty;
            // Damage Type
            if (effect is DamageEffect)
            {
                if (effect.TargetType == Enums.TargetType.Self)
                    effectDesc += $"Receive <color=#DF2510>{amount}</color><sprite=13> ";
                else
                    effectDesc += (effect.GetAmount() > 0) ? $"Deal <color=#DF2510>{amount}</color><sprite=13> " : $"Deal ";
            }
            else if (effect is DefenseEffect)
                effectDesc += $"Gain <color=#619BF3>{amount}</color><sprite=16> ";
            else if (effect is HealEffect)
                effectDesc += $"Heal <color=#68E4AA>{amount}</color><sprite=12> ";
            else if (effect is DrawCardEffect)
                effectDesc += $"Draw {amount} <sprite=9>.";
            else if (effect is EnergyEffect)
                effectDesc += $"Gain {amount} <sprite=5>.";
            else if (effect is ModifierEffect)
            {
                var buff = effect as ModifierEffect;
                var effectAmount = buff.Duration.HasValue ? buff.Duration.Value : buff.GetAmount();
                effectDesc += $"Apply {effectAmount} {buff.GetDisplayName()} ";
            }


            // Modifiers
            if (effect.StatType != Enums.Stat.None)
            {
                effectDesc += GetParsedModifier(effect.StatType);
            }

            // Target
            if (effect.TargetType == Enums.TargetType.SingleEnemy)
            {
                effectDesc += "to an enemy.";
            }
            else if (effect.TargetType == Enums.TargetType.MultipleEnemies)
            {
                effectDesc += "to all enemies.";
            }


            description += effectDesc + "\n";
        }
        if (_cardData.Exhausts())
            description += "Exhaust.";

        return description;
    }

    private string GetParsedModifier(Enums.Stat statType)
    {
        Stats stats = GameManager.Instance.Player.GetStats();
        if (stats != null)
        {
            if (stats.GetModifier(statType) > 0)
            {
                return $"<color=#0826A9>+ {stats.GetModifier(statType)} {ConvertToMod(statType)}</color> ";
            }
            else if (stats.GetModifier(statType) < 0)
            {
                return $"<color=#6F1309>{stats.GetModifier(statType)} {ConvertToMod(statType)}</color> ";
            }
            else
                return string.Empty;
        }
        else
        {
            return $"<color=#0826A9>+ {ConvertToMod(statType)}</color> ";
        }
    }
    private string ConvertToMod(Enums.Stat stat)
    {
        switch (stat)
        {
            default:
            case Enums.Stat.Strength:
                return "STR";
            case Enums.Stat.Dexterity:
                return "DEX";
            case Enums.Stat.Constitution:
                return "CON";
            case Enums.Stat.Intelligence:
                return "INT";
            case Enums.Stat.Wisdom:
                return "WIS";
            case Enums.Stat.Charisma:
                return "CHA";
        }
    }
}