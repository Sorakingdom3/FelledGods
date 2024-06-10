using TMPro;
using UnityEngine;

public class VictoryScreenController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _BannerText;

    public void Setup(bool victory)
    {
        if (victory)
        {
            _BannerText.SetText("Victory!");
        }
        else
        {
            _BannerText.SetText("Defeat");
        }
    }

    public void OnBackToMenuButtonPressed()
    {
        GameManager.Instance.EndGame();
        UIManager.Instance.BackToMenu();
    }
}
