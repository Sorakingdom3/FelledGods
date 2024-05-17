using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    [SerializeField] Button _previousButton;
    [SerializeField] Button _nextButton;
    [SerializeField] Image _characterSprite;
    [SerializeField] TextMeshProUGUI _characterDescription;
    [SerializeField] List<ClassData> _classes = new List<ClassData>();
    [SerializeField] Slider _slider;
    int _currentCharacter = 0;

    private void Awake()
    {
        if (_classes.Count > 0)
        {
            SetCharacter();
        }

        if (_classes.Count > 1)
        {
            _previousButton.interactable = true;
            _nextButton.interactable = true;
        }
    }

    public void OnNextButtonPressed()
    {
        _currentCharacter++;
        if (_currentCharacter >= _classes.Count)
        {
            _currentCharacter %= _classes.Count;
        }
    }
    public void OnPreviousButtonPressed()
    {
        _currentCharacter--;
        if (_currentCharacter < 0)
        {
            _currentCharacter = _classes.Count - 1;
        }

    }
    private void SetCharacter()
    {
        _characterDescription.text = $"{_classes[0].Name}\n\n" + $"{_classes[0].Description}";
        _characterSprite.sprite = _classes[0].Sprite;
    }
    public void OnSelectButtonPressed()
    {
        GameManager.Instance.SetClass(_classes[_currentCharacter], (int)_slider.value);
    }
}