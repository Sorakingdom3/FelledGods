using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject MainMenu;
    public GameObject CharacterSelection;
    public GameObject BattleArena;
    public GameObject Shop;
    public GameObject Inn;
    public GameObject Loot;
    public GameObject CardChoice;
    public GameObject Map;
    public GameObject CardList;
    public GameObject OptionsMenu;
    public GameObject MapBackButton;
    public GameObject VictoryScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void OnNewGameButtonPressed()
    {
        MainMenu.SetActive(false);
        CharacterSelection.SetActive(true);
    }
    public void OnCollectionButtonPressed()
    {
        GameManager.Instance.OpenCollection();
        CardList.SetActive(true);
    }
    public void OnOptionsButtonPressed()
    {
        OptionsMenu.SetActive(!OptionsMenu.activeSelf);
    }
    public void OnExitButtonPressed()
    {
        Application.Quit();
    }

    public void GoToBattle()
    {

        CharacterSelection.SetActive(false);
        BattleArena.SetActive(true);
        ShowMap(false);
    }
    public void HideMap()
    {
        Map.SetActive(false);
        MapBackButton.SetActive(false);
    }
    public void ShowMap(bool backButton)
    {
        Map.SetActive(true);
        MapBackButton.SetActive(backButton);
    }

    public void GotoCollection()
    {
        CardList.SetActive(true);
    }
    public void BackToMenu()
    {
        MainMenu.SetActive(true);
        CharacterSelection.SetActive(false);
        BattleArena.SetActive(false);
        Shop.SetActive(false);
        Inn.SetActive(false);
        Loot.SetActive(false);
        CardChoice.SetActive(false);
        Map.SetActive(false);
        CardList.SetActive(false);
        OptionsMenu.SetActive(false);
        VictoryScreen.SetActive(false);
    }

    public void OpenInn()
    {
        Inn.SetActive(true);
    }

    public void OpenShop()
    {
        Shop.SetActive(true);
    }

    public void OpenChest()
    {
        Loot.SetActive(true);
    }

    public void OpenCardChoice()
    {
        CardChoice.SetActive(true);
    }

    public void CloseCardChoice()
    {
        CardChoice.SetActive(false);
    }

    public void CloseChest()
    {
        Loot.SetActive(false);
    }

    public void OpenCardList()
    {
        CardList.SetActive(true);
    }
    public void CloseCardList()
    {
        CardList.SetActive(false);
    }

    public void ShowVictoryScreen(bool victory)
    {
        VictoryScreen.GetComponent<VictoryScreenController>().Setup(victory);
        VictoryScreen.SetActive(true);
    }
}