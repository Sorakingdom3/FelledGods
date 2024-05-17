using UnityEngine;

public class SelectableCard : MonoBehaviour
{
    [SerializeField] CardDisplay _display;
    Enums.CardListMode _mode;
    CardListController _controller;
    CardData _cardData;
    public void Set(CardListController controller, Enums.CardListMode mode, CardData card)
    {
        _controller = controller;
        _cardData = card;
        _display.Setup(_cardData);
        _mode = mode;
    }

    public void OnCardSelected()
    {
        _controller.OnCardSelected(_mode, _cardData);
    }
}
