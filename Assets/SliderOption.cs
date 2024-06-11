using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderOption : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _Value;
    [SerializeField] Slider _slider;

    private void Awake()
    {
        _slider.value = GameManager.Instance.AudioManager.GetVolume();
        _Value.SetText((_slider.value / _slider.maxValue * 100).ToString("N0"));
    }

    public void UpdateValue()
    {
        _Value.SetText((_slider.value / _slider.maxValue * 100).ToString("N0"));
        GameManager.Instance.AudioManager.SetVolume(_slider.value);
    }
}
