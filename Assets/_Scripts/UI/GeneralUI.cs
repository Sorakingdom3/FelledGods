using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneralUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI HealthText;
    [SerializeField] TextMeshProUGUI GoldText;
    [SerializeField] TextMeshProUGUI FloorText;
    [SerializeField] TextMeshProUGUI EnergyText;
    [SerializeField] TextMeshProUGUI HealthBarText;
    [SerializeField] Slider HealthBar;
    [SerializeField] GameObject DefenseBanner;
    [SerializeField] TextMeshProUGUI DefenseIconText;
    [SerializeField] TextMeshProUGUI DrawPileText;
    [SerializeField] TextMeshProUGUI DiscardPileText;

    [SerializeField] List<StatDisplay> statDisplays;

    [SerializeField] GameObject _buffPrefab;
    [SerializeField] Transform _buffContainer;
    List<BuffDisplay> _buffs = new List<BuffDisplay>();


    public void UpdateVisuals(int maxHealth, int health, int defense, int energy, int gold, Stats stats, int drawPileCount, int discardPileCount)
    {
        HealthText.SetText($"<sprite=21> {health}/{maxHealth}");
        HealthBarText.SetText($"{health}/{maxHealth}");
        HealthBar.maxValue = maxHealth;
        HealthBar.value = health;
        DefenseIconText.SetText(defense.ToString());
        DefenseBanner.SetActive(defense > 0);

        DrawPileText.SetText(drawPileCount.ToString());
        DiscardPileText.SetText(discardPileCount.ToString());

        GoldText.SetText($"<size=150%><sprite=18></size> {gold}");
        FloorText.SetText($"<size=150%><sprite=14></size> {GameManager.Instance.GetCurrentFloor() + 1}");
        EnergyText.SetText($"{energy}");

        foreach (StatDisplay display in statDisplays)
        {
            var statType = display.m_Stat;
            var total = stats.GetBase(statType);
            var mod = stats.GetModifier(statType);
            display.UpdateData(total, mod);
        }
    }

    public void Setup(ClassData classData)
    {
        NameText.SetText(classData.Name);
        UpdateVisuals(classData.BaseHealth, classData.BaseHealth, 0, classData.BaseEnergy, 0, classData.BaseStats, 0, 0);

    }

    // Start is called before the first frame update
    void Start()
    {
        statDisplays[0].Setup(Enums.Stat.Strength, 0, 0);
        statDisplays[1].Setup(Enums.Stat.Dexterity, 0, 0);
        statDisplays[2].Setup(Enums.Stat.Constitution, 0, 0);
        statDisplays[3].Setup(Enums.Stat.Intelligence, 0, 0);
        statDisplays[4].Setup(Enums.Stat.Wisdom, 0, 0);
        statDisplays[5].Setup(Enums.Stat.Charisma, 0, 0);
    }

    public void AddNewBuff(ModifierEffect effect)
    {
        GameObject buff = Instantiate(_buffPrefab, _buffContainer);
        var display = buff.GetComponent<BuffDisplay>();
        display.Setup(effect.GetBuffType(), effect.GetDisplayValue());
        _buffs.Add(display);
    }

    public void UpdateBuff(ModifierEffect effect)
    {
        var buff = _buffs.Find(b => b.GetBuffType() == effect.GetBuffType());
        if (buff != null)
        {
            buff.UpdateValue(effect.GetDisplayValue());
        }
    }

    public void RemoveBuff(Enums.BuffType buff)
    {
        BuffDisplay display = _buffs.Find(b => b.GetBuffType() == buff);
        _buffs.Remove(display);
        Destroy(display.gameObject);
    }

    public void ClearBuffs()
    {
        while (_buffs.Count > 0)
        {
            Destroy(_buffs[0].gameObject);
            _buffs.RemoveAt(0);
        }
    }
}
