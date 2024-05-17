using UnityEngine;

public class InnController : MonoBehaviour, IRoom
{
    [SerializeField] CardListController _cardListController;
    RoomManager _roomManager;
    public void OnSleepButtonPressed()
    {
        int max = GameManager.Instance.Player.GetMaxHealth();
        GameManager.Instance.Player.Heal((int)(max * .3f));
        OnActionCompleted();
    }
    public void OnEnchantButtonPressed()
    {
        _cardListController.gameObject.SetActive(true);
        _cardListController.OpenDeck(this, Enums.CardListMode.Enchant);
    }

    public void OnRemoveCardButtonPressed()
    {
        _cardListController.gameObject.SetActive(true);
        _cardListController.OpenDeck(this, Enums.CardListMode.Removal);
    }

    public void OnActionCompleted()
    {
        gameObject.SetActive(false);
        _roomManager.CompleteRoom();
    }

    public void OpenInn(RoomManager roomManager)
    {
        gameObject.SetActive(true);
        _roomManager = roomManager;
    }
}
