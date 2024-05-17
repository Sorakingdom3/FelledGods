using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Attributes

    public static GameManager Instance { get; set; }
    [SerializeField] OptionsManager OptionsManager;
    [SerializeField] public AudioManager AudioManager;
    [SerializeField] DeckManager DeckManager;
    [SerializeField] DataManager DataManager;
    [SerializeField] MapManager MapManager;
    [SerializeField] RoomManager RoomManager;
    [SerializeField] CardListController CardListController;

    List<List<Node>> _map;

    public Player Player;
    int _stage = 1;
    int _floor = 0;
    int _room = -1;

    Node _currentRoom;
    int _seed;
    public int _shopRemovals;

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {

        DataManager.LoadAllCards();

        Random.InitState(Random.Range(0, 1000000));
    }

    public void SetClass(ClassData classData, int difficulty)
    {
        _floor = 0;
        _room = -1;

        InitPlayer(classData);
        GenerateStage();
        UIManager.Instance.GoToBattle();
    }

    private void GenerateStage()
    {
        var mapGenerator = new MapGenerator();
        _map = mapGenerator.GenerateMap();
        _currentRoom = new Node(-1, -1);
        _currentRoom.Type = Enums.NodeType.Start;
        foreach (var child in _map[0])
        {
            if (child != null)
                _currentRoom.AddChild(child);
        }
        MapManager.DrawMap(_map);
    }

    public void InitPlayer(ClassData classData)
    {
        //Player = Instantiate(PlayerPrefab, _playerPosition).GetComponent<Player>();
        Player.Setup(classData);
        DeckManager.Setup(DataManager.GetBaseDeck(classData), classData.BaseHandSize);
        UIManager.Instance.GoToBattle();
    }
    public void VisitFloor(int floor, int room)
    {
        UIManager.Instance.HideMap();
        var visitedRoom = _map[floor][room];
        RoomManager.GenerateRoom(visitedRoom);
    }

    public void AdvanceFloor(Node node)
    {
        if (node.Type != Enums.NodeType.Boss)
        {
            _floor = node.Row;
            _room = node.Column;
            UIManager.Instance.ShowMap(false);
            MapManager.ProceedFrom(node);
        }
        else
        {
            GoToNextStage();
        }
    }

    private void GoToNextStage()
    {
        ++_stage;
        if (_stage == 3)
            EndRun();
        else
        {
            MapManager.ClearMap();
            GenerateStage();
            UIManager.Instance.ShowMap(false);
        }
    }

    public Node GetCurrentRoom()
    {
        return _currentRoom;
    }

    public void AddItem(Enums.LootType type, object item)
    {
        switch (type)
        {
            case Enums.LootType.Cards:
                var card = item as CardData;
                DeckManager.OnAddNewCardToDeck(card);
                break;
        }
    }

    public List<CardData> GetAllCards()
    {
        return DataManager.GetAllCards();
    }

    public void Heal(int amount)
    {
        Player.Heal(amount);
    }

    public void EndRun()
    {
        MapManager.ResetMap();

        UIManager.Instance.BackToMenu();
    }

    public List<CardData> GetDeck()
    {
        return DeckManager.Deck;
    }

    public void RemoveCardFromDeck(CardData data)
    {
        DeckManager.RemoveCardFromDeck(data);
    }

    public void OpenCollection()
    {
        CardListController.OpenCollection();
    }

    public int GetCurrentFloor()
    {
        return _floor;
    }

    public List<CardData> GetClassCards()
    {
        return DataManager.Instance.GetClassCards(Player.GetClass());
    }
}
