using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] Image _intentionIcon;
    [SerializeField] TextMeshProUGUI _intentionText;
    [SerializeField] TextMeshProUGUI _healthbarText;
    [SerializeField] Slider _healthbar;
    [SerializeField] TextMeshProUGUI _defenseText;
    [SerializeField] GameObject _defenseIconObject;

    [SerializeField] Sprite _damageIcon;
    [SerializeField] Sprite _defenseIcon;
    [SerializeField] Sprite _healIcon;
    public void Set(EnemyData enemyData)
    {
        _image.sprite = enemyData.Sprite;
        _healthbar.maxValue = enemyData.BaseHealth;
        _healthbar.value = enemyData.BaseHealth;
        UpdateVisuals((int)_healthbar.value, (int)_healthbar.value, 0);
        _defenseIconObject.SetActive(false);
    }

    public void UpdateVisuals(int hp, int maxHp, int defense)
    {
        _healthbar.value = hp;
        _healthbarText.SetText($"{hp}/{maxHp}");
        _defenseText.SetText(defense.ToString());
        _defenseIconObject.gameObject.SetActive(defense > 0);
    }

    public void SetIntention(Effect intendedAction, Stats stats)
    {
        if (intendedAction is DamageEffect)
        {
            _intentionIcon.sprite = _damageIcon;
        }
        else if (intendedAction is DefenseEffect)
        {
            _intentionIcon.sprite = _defenseIcon;

        }
        else if (intendedAction is HealEffect)
        {
            _intentionIcon.sprite = _healIcon;

        }
        if (intendedAction.HasStatModifier)
        {
            int amount = intendedAction.Amount.BaseAmount + stats.GetModifier(intendedAction.StatType);
            _intentionText.SetText(amount.ToString());
        }
        else
        {
            _intentionText.SetText(intendedAction.Amount.BaseAmount.ToString());
        }
    }
}