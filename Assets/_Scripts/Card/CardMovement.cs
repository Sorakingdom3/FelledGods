using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] RectTransform _rectTransform;
    [SerializeField] CardDisplay _card;
    Player _player;
    Canvas _canvas;
    Vector2 OriginalPointerLocalPosition;
    Vector3 OriginalPanelLocalPosition;
    Vector3 _originalScale;
    int _currentState = 0;
    Quaternion _originalRotation;
    Vector3 _originalPosition;

    [SerializeField] float _selectScale = 1.1f;
    [SerializeField] float _lerpFactor = 0.1f;
    [SerializeField] Vector2 _cardPlay;
    [SerializeField] Vector3 _playPosition;
    [SerializeField] GameObject _glowEffect;
    [SerializeField] GameObject _playArrow;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _canvas = GetComponentInParent<Canvas>();
        _originalScale = _rectTransform.localScale;
        _originalPosition = _rectTransform.localPosition;
        _originalRotation = _rectTransform.localRotation;
    }

    private void Update()
    {
        switch (_currentState)
        {
            case 1:
                HandleHover();
                break;
            case 2:
                HandleMovement();
                if (Input.GetMouseButtonDown(0) && _rectTransform.localPosition.y > _cardPlay.y && !(_card._cardData.Type == Enums.CardType.Weapon || _card._cardData.Type == Enums.CardType.Spell))
                {
                    _player.PlayCard(_card, _player, _player);
                }
                else if (Input.GetMouseButtonDown(0) && !(_rectTransform.localPosition.y > _cardPlay.y))
                {
                    Unselect();
                    TransitionToState0();
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    Unselect();
                    TransitionToState0();
                }
                break;
            case 3:
                HandleAttackMovement();
                if (Input.GetMouseButtonDown(1))
                {
                    Unselect();
                    TransitionToState0();
                }
                break;
            default:
                break;
        }
    }

    private void HandleAttackMovement()
    {
        _rectTransform.localPosition = Vector3.Lerp(_rectTransform.localPosition, _playPosition, _lerpFactor);
        _rectTransform.localRotation = Quaternion.identity;
        if (Input.mousePosition.y < _cardPlay.y)
        {
            _currentState = 2;
            _rectTransform.position = Input.mousePosition;
            _playArrow.SetActive(false);
        }
    }

    private void HandleMovement()
    {
        _rectTransform.localRotation = Quaternion.identity;
        if (_currentState == 2)
        {
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.GetComponent<RectTransform>(), Camera.main.WorldToScreenPoint(Input.mousePosition), Camera.main, out localPointerPosition))
            {
                _rectTransform.position = Vector3.Lerp(_rectTransform.position, Input.mousePosition, _lerpFactor);

                if (_rectTransform.localPosition.y > _cardPlay.y && (_card._cardData.Type == Enums.CardType.Weapon || _card._cardData.Type == Enums.CardType.Spell))
                {
                    _currentState = 3;
                    _playArrow.SetActive(true);
                    _rectTransform.localPosition = Vector3.Lerp(_rectTransform.localPosition, _playPosition, _lerpFactor);
                }
            }
        }
    }

    private void HandleHover()
    {
        // Logica de Poner en Visor
        _glowEffect.SetActive(true);
        _rectTransform.localScale = _originalScale * _selectScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_currentState == 0 && _player.IsPlayable(_card._cardData))
        {
            _originalPosition = _rectTransform.localPosition;
            _originalRotation = _rectTransform.localRotation;
            _originalScale = _rectTransform.localScale;

            _currentState = 1;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_currentState == 1)
        {
            TransitionToState0();
        }
    }
    private void TransitionToState0()
    {
        // Reset Everything
        _currentState = 0;
        _rectTransform.localScale = _originalScale;
        _rectTransform.localRotation = _originalRotation;
        _rectTransform.localPosition = _originalPosition;
        _player._battleController.EnemyClickedEvent.RemoveListener(PlayCard);
        _glowEffect.SetActive(false);
        _playArrow.SetActive(false);
    }
    private void PlayCard(Enemy enemy)
    {
        _player._battleController.EnemyClickedEvent.RemoveListener(PlayCard);
        Unselect();
        _player.PlayCard(_card, _player, enemy);
    }

    public void Unselect()
    {
        TransitionToState0();
        _player._battleController.EnemyClickedEvent.RemoveListener(PlayCard);
        _player.SelectedCard = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentState == 1)
        {
            _currentState = 2;
            if (_player.SelectedCard != null)
            {
                _player.SelectedCard.Unselect();
            }
            _player.SelectedCard = this;
            _player._battleController.EnemyClickedEvent.AddListener(PlayCard);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out OriginalPointerLocalPosition);
            OriginalPanelLocalPosition = _rectTransform.localPosition;
        }
    }
}
