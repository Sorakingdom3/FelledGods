using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardListController : MonoBehaviour
{
    [SerializeField] GameObject _selectableCardPrefab;
    [SerializeField] Transform _container;
    List<GameObject> _cards = new List<GameObject>();
    IRoom _parentCall;
    public void OpenDeck(IRoom room, Enums.CardListMode mode)
    {
        _parentCall = room;
        var deck = GameManager.Instance.GetDeck();
        if (mode == Enums.CardListMode.Enchant)
            deck = deck.Where(c => c.Level != Enums.ModifierType.Enchanted).ToList();
        foreach (var card in deck)
        {
            var display = Instantiate(_selectableCardPrefab, _container);
            _cards.Add(display);
            var selectable = display.GetComponent<SelectableCard>();
            selectable.Set(this, mode, card);
        }
    }

    public void OpenCollection()
    {
        var collection = GameManager.Instance.GetAllCards();
        foreach (var card in collection)
        {
            var display = Instantiate(_selectableCardPrefab, _container);
            _cards.Add(display);
            var selectable = display.GetComponent<SelectableCard>();
            selectable.Set(this, Enums.CardListMode.Collection, card);
        }
    }

    public void OnCardSelected(Enums.CardListMode mode, CardData data)
    {
        switch (mode)
        {
            case Enums.CardListMode.Removal:
                GameManager.Instance.RemoveCardFromDeck(data);
                _parentCall.OnActionCompleted();
                _parentCall = null;
                UIManager.Instance.CloseCardList();
                ClearList();
                break;
            case Enums.CardListMode.Enchant:
                data.Enchant();
                _parentCall.OnActionCompleted();
                _parentCall = null;
                UIManager.Instance.CloseCardList();
                ClearList();
                break;
            default:
                break;
        }
    }

    public void OnExitButtonPressed()
    {
        ClearList();
        UIManager.Instance.CloseCardList();
    }

    private void ClearList()
    {
        foreach (var card in _cards)
        {
            Destroy(card);
        }
        _cards.Clear();
    }
}
