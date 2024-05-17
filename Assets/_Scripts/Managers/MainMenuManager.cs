using System.IO;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    //public UIManager UIManager;

    private SaveFile _saveFile;

    public void Start()
    {
        CheckForSavedGame();
    }

    private void CheckForSavedGame()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Saves/save.json");
        if (File.Exists(path))
        {
            try
            {
                string json = File.ReadAllText(path);
                var saveFile = JsonUtility.FromJson<SaveFile>(json);
                //if (saveFile.IsUsableData())
                //{
                //    _saveFile = saveFile;
                //    UIManager.ShowContinueButton();
                //}
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    public void OnContinueButtonPressed()
    {
        LoadSavedGame();
    }


    public void OnNewGameButtonPressed()
    {
        var seed = Random.Range(0, 100000);
        Random.InitState(seed);
        GoToCharacterSelection();
    }

    public void OnCollectionButtonPressed()
    {
        GoToCardCollection();
    }

    public void OnExitGameButtonPressed()
    {
        Application.Quit();
    }

    private void GoToCardCollection()
    {
        throw new System.NotImplementedException();
    }

    private void LoadSavedGame()
    {
        throw new System.NotImplementedException();
    }

    private void GoToCharacterSelection()
    {

    }
}
