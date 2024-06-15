using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownHandler : MonoBehaviour
{
    List<string> _FrameRates = new List<string>() { "120", "60", "30" };

    public TMP_Dropdown Dropdown;

    private void Start()
    {
        Dropdown.AddOptions(_FrameRates);
        Dropdown.SetValueWithoutNotify(0);
        Dropdown.onValueChanged.AddListener(OnNewFrameRateSelected);
        Application.targetFrameRate = int.Parse(Dropdown.options[0].text);
    }

    private void OnNewFrameRateSelected(int index)
    {
        Application.targetFrameRate = int.Parse(Dropdown.options[index].text);
    }
}
