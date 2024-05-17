using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [SerializeField] List<ClassData> classDataList = new List<ClassData>();
    [SerializeField] List<EncounterData> encounterDataList = new List<EncounterData>();

    List<CardData> _allCards = new List<CardData>();
    List<EncounterData> _usedEncounters = new List<EncounterData>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void LoadAllCards()
    {
        foreach (var classData in classDataList)
        {
            var cards = classData.ClassCards;
            foreach (var card in cards)
            {
                var clone = CardData.CopyOf(card);
                _allCards.Add(clone);
            }
        }
    }

    public List<CardData> GetAllCards()
    {
        return _allCards;
    }

    public List<CardData> GetBaseDeck(ClassData playerClass)
    {
        var deck = new List<CardData>();
        foreach (var card in playerClass.BaseDeck)
        {
            var clone = CardData.CopyOf(card);
            deck.Add(clone);
        }
        return deck;
    }

    public EncounterData GetEncounter(Enums.NodeType type)
    {
        var elegibles = new List<EncounterData>();
        foreach (var encounter in encounterDataList)
        {
            if (!_usedEncounters.Contains(encounter) && encounter.Type == type)
                elegibles.Add(encounter);
        }
        if (elegibles.Count == 0)
        {
            _usedEncounters.Clear();
            return GetEncounter(type);
        }
        return elegibles[Random.Range(0, elegibles.Count)];
    }

    public List<CardData> GetClassCards(ClassData classData)
    {
        var elegibles = new List<CardData>();
        foreach (var card in classData.ClassCards)
        {
            elegibles.Add(CardData.CopyOf(card));
        }
        return elegibles;
    }
}
