using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] ShopController _shopManager;
    [SerializeField] InnController _innController;
    [SerializeField] BattleController _battleManager;
    [SerializeField] LootController _lootController;

    Node _room;
    public void GenerateRoom(Node room)
    {
        _room = room;

        switch (_room.Type)
        {
            case Enums.NodeType.Event:
                GenerateNewEvent();
                break;
            case Enums.NodeType.Shop:
                GenerateNewShop();
                break;
            case Enums.NodeType.Fight:
                GenerateNewFight(_room.Type);
                break;
            case Enums.NodeType.Inn:
                OpenInn();
                break;
            case Enums.NodeType.Chest:
                GenerateNewChest(Enums.NodeType.Chest);
                break;
            case Enums.NodeType.Elite:
                GenerateNewFight(_room.Type);
                break;
            case Enums.NodeType.Boss:
                GenerateNewFight(_room.Type);
                break;

        }
    }

    public void GenerateNewChest(Enums.NodeType node)
    {
        _lootController.OpenNewChest(this, node);
    }

    private void OpenInn()
    {
        _innController.OpenInn(this);
    }

    private void GenerateNewFight(Enums.NodeType type)
    {
        _battleManager.GenerateFight(this, type);
    }

    private void GenerateNewShop()
    {
        _shopManager.GenerateShop(this);
    }

    private void GenerateNewEvent()
    {
        List<Enums.NodeType> events = new List<Enums.NodeType>
        {
            Enums.NodeType.Chest,
            Enums.NodeType.Fight,
            Enums.NodeType.Shop,
            Enums.NodeType.Inn
        };
        var chosen = events[Random.Range(0, events.Count)];
        switch (chosen)
        {
            case Enums.NodeType.Chest:
                GenerateNewChest(Enums.NodeType.Chest);
                break;
            case Enums.NodeType.Fight:
                GenerateNewFight(Enums.NodeType.Fight);
                break;
            case Enums.NodeType.Shop:
                GenerateNewShop();
                break;
            case Enums.NodeType.Inn:
                OpenInn();
                break;
        }
    }

    public void CompleteRoom()
    {
        _room.Completed = true;
        GameManager.Instance.AdvanceFloor(_room);
    }

}