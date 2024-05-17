using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{

    [SerializeField] private Slider _volumeBar;
    public void OnVolumeValueChanged()
    {
        GameManager.Instance.AudioManager.SetVolume(_volumeBar.value);
    }
}
