using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private GameObject _LeaveRunButton;

    [SerializeField] private Slider _volumeBar;

    private void OnEnable()
    {
        if (GameManager.Instance.HasRunStarted())
            _LeaveRunButton.SetActive(true);
        else
            _LeaveRunButton.SetActive(false);
    }

    public void OnVolumeValueChanged()
    {
        GameManager.Instance.AudioManager.SetVolume(_volumeBar.value);
    }
}
