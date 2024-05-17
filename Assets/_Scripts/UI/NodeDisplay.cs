using UnityEngine;
using UnityEngine.UI;

public class NodeDisplay : MonoBehaviour
{
    [Header("Node Sprites")]
    [SerializeField] Sprite _eventIcon;
    [SerializeField] Sprite _chestIcon;
    [SerializeField] Sprite _shopIcon;
    [SerializeField] Sprite _innIcon;
    [SerializeField] Sprite _fightIcon;
    [SerializeField] Sprite _eliteIcon;
    [SerializeField] Sprite _bossIcon;

    [Header("UI Elements")]
    [SerializeField] Image _icon;
    [SerializeField] GameObject _visitedIcon;
    [SerializeField] Button _button;

    int _row;
    int _column;
    MapManager _mapManager;
    public Node Node;

    bool growing = false;
    Vector2 originalSize;
    Vector2 TopSize;
    RectTransform _rectTransform;

    private void OnEnable()
    {
        _rectTransform = GetComponent<RectTransform>();
        originalSize = _rectTransform.sizeDelta;
        TopSize = originalSize * 1.5f;
    }
    public void Setup(MapManager manager, Node node)
    {
        _row = node.Row;
        _column = node.Column;
        _mapManager = manager;
        Node = node;
        _button.interactable = false;
        if (node.Completed)
        {
            _visitedIcon.SetActive(true);
        }

        switch (node.Type)
        {
            case Enums.NodeType.Event:
                _icon.sprite = _eventIcon;

                break;
            case Enums.NodeType.Inn:
                _icon.sprite = _innIcon;

                break;
            case Enums.NodeType.Shop:
                _icon.sprite = _shopIcon;

                break;
            case Enums.NodeType.Elite:
                _icon.sprite = _eliteIcon;

                break;
            case Enums.NodeType.Boss:
                _icon.sprite = _bossIcon;

                break;
            case Enums.NodeType.Fight:
                _icon.sprite = _fightIcon;

                break;
            case Enums.NodeType.Chest:
                _icon.sprite = _chestIcon;

                break;
        }
    }

    public void OnRoomSelected()
    {
        _visitedIcon.SetActive(true);
        _mapManager.RunNode(_row, _column);
        _button.interactable = false;
    }

    public void Open()
    {
        _button.interactable = true;
    }

    private void Update()
    {
        if (_button.interactable)
        {
            if (growing)
                _rectTransform.sizeDelta = Vector2.Lerp(_rectTransform.sizeDelta, TopSize, .01f);
            else
                _rectTransform.sizeDelta = Vector2.Lerp(_rectTransform.sizeDelta, originalSize, .01f);

            if ((growing && TopSize.y - _rectTransform.sizeDelta.y < 5) || (!growing && _rectTransform.sizeDelta.y - originalSize.y < 5))
            {
                growing = !growing;
            }
        }
    }

    public void Close()
    {
        _button.interactable = false;
    }

}
