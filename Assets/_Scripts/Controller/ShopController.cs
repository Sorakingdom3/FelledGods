using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour, IRoom
{
    List<ShopItem> _shopItems = new List<ShopItem>();
    RoomManager _roomManager;
    [SerializeField] GameObject _shopItemPrefab;
    [SerializeField] Button _removalButton;
    [SerializeField] Transform _shopContainer;
    [SerializeField] TextMeshProUGUI _removalButtonText;
    [SerializeField] CardListController _cardListController;
    int _removalPrice;

    public void GenerateShop(RoomManager roomManager)
    {
        _roomManager = roomManager;
        gameObject.SetActive(true);
        var classCards = DataManager.Instance.GetClassCards(GameManager.Instance.Player.GetClass());
        var common = classCards.Where(c => c.Rarity == Enums.RarityType.Common).ToList();
        var rare = classCards.Where(c => c.Rarity == Enums.RarityType.Rare).ToList();
        var legendary = classCards.Where(c => c.Rarity == Enums.RarityType.Legendary).ToList();
        for (int i = 0; i < 7; i++)
        {
            var chosenCard = common[Random.Range(0, common.Count())];
            common.Remove(chosenCard);
            var item = Instantiate(_shopItemPrefab, _shopContainer).GetComponent<ShopItem>();
            item.Setup(Enums.LootType.Cards, chosenCard, Random.Range(45, 55));
            _shopItems.Add(item);
        }
        for (int i = 0; i < 3; i++)
        {
            var chosenCard = rare[Random.Range(0, rare.Count())];
            rare.Remove(chosenCard);
            var item = Instantiate(_shopItemPrefab, _shopContainer).GetComponent<ShopItem>();
            item.Setup(Enums.LootType.Cards, chosenCard, Random.Range(45, 55));
            _shopItems.Add(item);
        }

        _removalButton.interactable = (GameManager.Instance.Player.Gold >= _removalPrice);
        _removalPrice = 75 + 25 * GameManager.Instance._shopRemovals;
        _removalButtonText.SetText($"Card \nRemoval\n{_removalPrice}");
    }

    public void OnActionCompleted()
    {
        GameManager.Instance._shopRemovals++;
        _removalButton.interactable = false;
    }
    public void OnShopRemovalButtonPressed()
    {
        if (GameManager.Instance.Player.Gold >= _removalPrice)
        {
            UIManager.Instance.OpenCardList();
            _cardListController.OpenDeck(this, Enums.CardListMode.Removal);
        }
    }
    public void OnExitShopButtonPressed()
    {
        ClearShop();
        gameObject.SetActive(false);
        _roomManager.CompleteRoom();
    }

    private void ClearShop()
    {
        foreach (var item in _shopItems)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        _shopItems.Clear();
    }
}
