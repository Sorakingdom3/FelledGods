using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    int _price = 0;
    Enums.LootType _type;
    object _item;
    [SerializeField] TextMeshProUGUI _priceTag;
    [SerializeField] CardDisplay _display;
    [SerializeField] Button _button;

    public void Setup(Enums.LootType type, object item, int amount)
    {
        _type = type;
        _item = item;
        _display.Setup(item as CardData);
        _price = amount;
        _priceTag.SetText(amount.ToString());
    }

    public void OnBuyButtonPressed()
    {
        if (GameManager.Instance.Player.Gold >= _price)
        {
            GameManager.Instance.Player.SubtractGold(_price);
            GameManager.Instance.AddItem(_type, _item);
            _button.interactable = false;
        }
    }
}
