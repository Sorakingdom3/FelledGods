using TMPro;
using UnityEngine;

public class LootController : MonoBehaviour
{
    [SerializeField] int _bossMaxGold;
    [SerializeField] int _bossMinGold;
    [SerializeField] int _eliteMaxGold;
    [SerializeField] int _eliteMinGold;
    [SerializeField] int _fightMaxGold;
    [SerializeField] int _fightMinGold;

    [SerializeField] GameObject _goldButton;
    [SerializeField] GameObject _cardCommon;
    [SerializeField] GameObject _cardRare;
    [SerializeField] GameObject _cardLegendary;
    [SerializeField] TextMeshProUGUI _goldButtonText;

    [SerializeField] CardChoiceController _cardChoiceController;

    RoomManager _roomManager;
    int _gold;

    public void OpenNewChest(RoomManager roomManager, Enums.NodeType node)
    {
        _roomManager = roomManager;
        _goldButton.SetActive(false);
        _cardRare.SetActive(false);
        _cardCommon.SetActive(false);
        _cardLegendary.SetActive(false);

        switch (node)
        {
            case Enums.NodeType.Boss:
                _gold = Random.Range(_bossMinGold, _bossMaxGold);
                _cardLegendary.SetActive(true);
                _cardRare.SetActive(true);
                _cardCommon.SetActive(true);
                break;
            case Enums.NodeType.Chest:
            case Enums.NodeType.Elite:
                _gold = Random.Range(_eliteMinGold, _eliteMaxGold);
                _cardRare.SetActive(true);
                _cardCommon.SetActive(true);
                break;
            case Enums.NodeType.Fight:
            default:
                _gold = Random.Range(_fightMinGold, _fightMaxGold);
                _cardCommon.SetActive(true);
                break;
        }
        _goldButtonText.SetText($"<sprite=18> {_gold} Gold coins");
        _goldButton.SetActive(true);
        gameObject.SetActive(true);
    }

    public void OnGoldButtonPressed()
    {
        GameManager.Instance.Player.AddGold(_gold);
        _goldButton.SetActive(false);
    }

    public void OnContinueButtonPressed()
    {
        UIManager.Instance.CloseChest();
        _roomManager.CompleteRoom();
    }

    public void OnCardButtonPressed(int rarity)
    {
        if (rarity == 0)
            _cardCommon.SetActive(false);
        else if (rarity == 1)
            _cardRare.SetActive(false);
        else if (rarity == 2)
            _cardLegendary.SetActive(false);
        _cardChoiceController.SetChoices(rarity);
        UIManager.Instance.OpenCardChoice();
    }
}
