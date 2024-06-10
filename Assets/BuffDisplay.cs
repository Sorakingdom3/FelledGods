using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _counter;
    [SerializeField] Image _BuffIcon;
    Enums.BuffType _BuffType;


    [SerializeField] List<Enums.BuffType> _BuffTypes;
    [SerializeField] List<Sprite> _BuffSprites;

    public void Setup(Enums.BuffType type, int value)
    {
        int buffIdx = _BuffTypes.IndexOf(type);
        _BuffIcon.sprite = _BuffSprites[buffIdx];
        _counter.SetText(value.ToString());
        _BuffType = type;
    }

    public void UpdateValue(int value)
    {
        _counter.SetText(value.ToString());
    }

    public Enums.BuffType GetBuffType()
    {
        return _BuffType;
    }
}
