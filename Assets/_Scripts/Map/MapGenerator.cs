using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator
{
    private const float XPixelDist = 150;
    private const float YPixelDist = 200;
    private const int MaxRandomVariation = 30;
    private const int FloorAmount = 15;
    private const int RoomsPerFloor = 5;
    private const int PathNumber = 5;

    private const float FightRoomWeight = 45f;
    private const float EventRoomWeight = 22f;
    private const float EliteRoomWeight = 16f;
    private const float InnRoomWeight = 12f;
    private const float ShopRoomWeight = 5f;

    private Dictionary<Enums.NodeType, float> RandomRoomWeights = new Dictionary<Enums.NodeType, float>
    {

        {Enums.NodeType.Fight,0.0f },
        {Enums.NodeType.Event, 0.0f},
        {Enums.NodeType.Elite, 0.0f},
        {Enums.NodeType.Inn, 0.0f},
        {Enums.NodeType.Shop, 0.0f}
    };
    private float RandomRoomTypeTotalWeight = 0f;
    private List<List<Node>> MapData = new List<List<Node>>();

    public MapGenerator()
    {


    }

    public List<List<Node>> GenerateMap()
    {
        MapData = GenFullMap();
        var startingPoints = GetStartingPoints();
        foreach (int j in startingPoints)
        {
            var currentJ = j;
            for (int i = 0; i < FloorAmount - 1; i++)
            {
                currentJ = SetConnection(i, currentJ);
            }
        }
        SetRandomWeights();
        SetRoomTypes();
        SetBoss();
        DeleteUnusedNodes();
        //PrintMap(MapData);

        return MapData;
    }

    private void DeleteUnusedNodes()
    {
        foreach (var floor in MapData)
            for (int i = 0; i < floor.Count; i++)
                if (floor[i].Type == Enums.NodeType.Undefined)
                    floor[i] = null;
    }

    private void SetRoomTypes()
    {
        foreach (var room in MapData[0])
        {
            if (room.Children.Count > 0) room.Type = Enums.NodeType.Fight;
        }
        foreach (var room in MapData[8])
        {
            if (room.Children.Count > 0) room.Type = Enums.NodeType.Chest;
        }
        foreach (var room in MapData[13])
        {
            if (room.Children.Count > 0)
            {
                foreach (var child in room.Children)
                {
                    if (child.Type == Enums.NodeType.Undefined)
                        child.Type = Enums.NodeType.Inn;
                }
            }

        }
        foreach (var floor in MapData)
        {
            foreach (var room in floor)
            {
                List<Enums.NodeType> usedCandidates = new List<Enums.NodeType>();
                foreach (var nextRoom in room.Children)
                {
                    if (nextRoom.Type == Enums.NodeType.Undefined)
                    {
                        usedCandidates.Add(SetRoomTypeRandomly(nextRoom, usedCandidates));
                    }
                }
            }
        }
    }

    private Enums.NodeType SetRoomTypeRandomly(Node room, List<Enums.NodeType> usedCandidates)
    {
        bool innBelowFloor6 = true;
        bool consecutiveInn = true;
        bool consecutiveShop = true;
        bool consecutiveElite = true;
        bool elitebelowFloor6 = true;
        bool innOnFloor13 = true;
        bool isNotUnique = true;
        bool shopBlockBelow6 = true;
        Enums.NodeType typeCandidate = Enums.NodeType.Undefined;
        while (shopBlockBelow6 || isNotUnique || innBelowFloor6 || consecutiveInn || consecutiveShop || innOnFloor13 || consecutiveElite || elitebelowFloor6)
        {
            typeCandidate = GetRandomCandidateByWeight();
            bool isInn = typeCandidate == Enums.NodeType.Inn;
            bool hasInnParent = HasParentOfType(room, Enums.NodeType.Inn);
            bool isShop = typeCandidate == Enums.NodeType.Shop;
            bool hasShopParent = HasParentOfType(room, Enums.NodeType.Shop);
            bool isElite = typeCandidate == Enums.NodeType.Elite;
            bool hasEliteParent = HasParentOfType(room, Enums.NodeType.Elite);

            shopBlockBelow6 = isShop && room.Children.Count >= 3 && room.Row < 5;
            isNotUnique = usedCandidates.Contains(typeCandidate);
            innBelowFloor6 = isInn && room.Row < 5;
            consecutiveInn = isInn && hasInnParent;
            consecutiveShop = isShop && hasShopParent;
            consecutiveElite = isElite && hasEliteParent;
            elitebelowFloor6 = isElite && room.Row < 5;
            innOnFloor13 = isInn && room.Row == 13;

        }
        room.Type = typeCandidate;
        return typeCandidate;
    }

    private bool HasParentOfType(Node room, Enums.NodeType type)
    {
        bool match = false;
        if (room.Row == 0)
            return false;

        if (room.Column > 0)
            match = MapData[room.Row - 1][room.Column - 1].Children.Contains(room) && MapData[room.Row - 1][room.Column - 1].Type == type;

        if (room.Column < RoomsPerFloor - 1)
            match = match || (MapData[room.Row - 1][room.Column + 1].Children.Contains(room) && MapData[room.Row - 1][room.Column + 1].Type == type);

        match = match || (MapData[room.Row - 1][room.Column].Children.Contains(room) && MapData[room.Row - 1][room.Column].Type == type);

        return match;
    }

    private Enums.NodeType GetRandomCandidateByWeight()
    {
        var roll = Random.Range(0f, RandomRoomTypeTotalWeight);
        foreach (var type in RandomRoomWeights.Keys)
        {
            if (RandomRoomWeights[type] > roll)
                return type;
        }
        return Enums.NodeType.Fight;
    }

    private void SetRandomWeights()
    {
        RandomRoomWeights[Enums.NodeType.Fight] = FightRoomWeight;
        RandomRoomWeights[Enums.NodeType.Event] = FightRoomWeight + EventRoomWeight;
        RandomRoomWeights[Enums.NodeType.Elite] = FightRoomWeight + EventRoomWeight + EliteRoomWeight;
        RandomRoomWeights[Enums.NodeType.Inn] = FightRoomWeight + EventRoomWeight + EliteRoomWeight + InnRoomWeight;
        RandomRoomWeights[Enums.NodeType.Shop] = FightRoomWeight + EventRoomWeight + EliteRoomWeight + InnRoomWeight + ShopRoomWeight;
        RandomRoomTypeTotalWeight += RandomRoomWeights[Enums.NodeType.Shop];
    }

    private void SetBoss()
    {
        MapData.Add(new List<Node>());
        Node boss = new Node(MapData.Count - 1, RoomsPerFloor / 2);
        for (int i = 0; i < RoomsPerFloor / 2; ++i)
        {
            MapData[MapData.Count - 1].Add(new Node(MapData.Count - 1, i));
        }
        MapData[MapData.Count - 1].Add(boss);
        for (int j = 0; j < RoomsPerFloor; j++)
        {
            var currentRoom = MapData[FloorAmount - 1][j];
            currentRoom.Children.Clear();
            currentRoom.AddChild(boss);
        }
        boss.Position = new Vector2((boss.Column - RoomsPerFloor / 2) * XPixelDist, boss.Row * YPixelDist);

        boss.Type = Enums.NodeType.Boss;
    }

    private int SetConnection(int i, int j)
    {
        Node nextRoom = null;
        Node currentRoom = MapData[i][j];
        while (nextRoom == null || CrossesExistingPath(i, j, nextRoom))
        {
            var rand_j = -1;
            while (rand_j < 0)
            {
                rand_j = Random.Range(j - 1, j + 2);
                if (rand_j < 0 || rand_j >= RoomsPerFloor) rand_j = -1;
            }
            nextRoom = MapData[i + 1][rand_j];
        }
        currentRoom.AddChild(nextRoom);
        return nextRoom.Column;
    }

    private bool CrossesExistingPath(int i, int j, Node room)
    {
        Node leftNeighbour = null;
        Node rightNeighbour = null;
        if (j > 0) leftNeighbour = MapData[i][j - 1];
        if (j < RoomsPerFloor - 1) rightNeighbour = MapData[i][j + 1];

        if (rightNeighbour != null && room.Column > j)
        {
            if (rightNeighbour.Children.FirstOrDefault(c => c.Column < room.Column) != null)
                return true;
        }
        if (leftNeighbour != null && room.Column < j)
        {
            if (leftNeighbour.Children.FirstOrDefault(c => c.Column > room.Column) != null)
                return true;
        }

        return false;
    }

    private List<int> GetStartingPoints()
    {
        var result = new List<int>();
        int uniquePoints = 0;
        while (uniquePoints < 2)
        {
            result = new List<int>();
            uniquePoints = 0;
            for (int i = 0; i < PathNumber; i++)
            {
                var j = Random.Range(0, RoomsPerFloor);
                if (!result.Contains(j))
                    uniquePoints++;
                result.Add(j);
            }
        }
        return result;
    }

    private void PrintMap(List<List<Node>> map)
    {
        string s = "";
        for (int i = 0; i < map.Count; i++)
        {
            s += $"Floor {i}:   \t";
            for (int j = 0; j < map[i].Count; j++)
            {
                if (map[i][j] != null)
                    s += map[i][j].ToString() + ", ";
            }
            s += "\n";
        }
        Debug.Log(s);
    }

    private List<List<Node>> GenFullMap()
    {
        var map = new List<List<Node>>();
        for (int i = 0; i < FloorAmount; i++)
        {
            map.Add(new List<Node>());
            for (int j = 0; j < RoomsPerFloor; j++)
            {
                var newRoom = new Node(i, j);
                Vector2 offset = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)) * MaxRandomVariation;
                newRoom.Position = new Vector2((j - RoomsPerFloor / 2) * XPixelDist, i * YPixelDist) + offset;
                map[i].Add(newRoom);
            }
        }
        return map;
    }
}
