using System.Collections.Generic;
using UnityEngine;

public class CardChoiceController : MonoBehaviour
{
    [SerializeField] List<CardDisplay> choiceList;

    public void SetChoices(int rarity)
    {
        var classCards = GameManager.Instance.GetClassCards();
        if (rarity == 0)
            classCards = classCards.FindAll(c => c.Rarity == Enums.RarityType.Common);
        else if (rarity == 1)
            classCards = classCards.FindAll(c => c.Rarity == Enums.RarityType.Rare);
        else if (rarity == 2)
            classCards = classCards.FindAll(c => c.Rarity == Enums.RarityType.Legendary);

        foreach (var card in choiceList)
        {
            var data = classCards[Random.Range(0, classCards.Count)];
            card.Setup(CardData.CopyOf(data));
            classCards.Remove(data);
        }
    }

    public void OnCardChosen(int choicePosition)
    {
        GameManager.Instance.AddItem(Enums.LootType.Cards, choiceList[choicePosition]._cardData);
        UIManager.Instance.CloseCardChoice();
    }
    public void OnChoiceSkipped()
    {
        UIManager.Instance.CloseCardChoice();
    }
}
