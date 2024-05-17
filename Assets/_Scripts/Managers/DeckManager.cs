using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] Transform HandContainer;
    [SerializeField] GameObject CardPrefab;

    public float m_FanSpread;
    public float m_HorizontalSpacing;
    public float m_VerticalSpacing = 100f;

    public int m_HandSize;
    public List<CardData> Deck;

    public List<CardData> DiscardPile;
    public List<CardData> DrawPile;
    public List<CardDisplay> Hand;

    public void Setup(List<CardData> deck, int baseHandSize)
    {
        Deck = deck;
        DrawPile = new List<CardData>();
        DrawPile.AddRange(deck);
        DiscardPile = new List<CardData>();
        Hand = new List<CardDisplay>();
        m_HandSize = baseHandSize;
    }

    public void SetForBattle()
    {
        ResetDiscardPile();
        DrawPile.Clear();
        DrawPile.AddRange(Deck);
        ShuffleDrawPile();
    }

    public void DrawCard()
    {
        if (DrawPile.Count == 0)
        {
            ResetDiscardPile();
            ShuffleDrawPile();
        }

        var cardData = DrawPile[0];
        DrawPile.RemoveAt(0);
        var card = Instantiate(CardPrefab, HandContainer);
        var display = card.GetComponent<CardDisplay>();
        display.Setup(cardData);
        Hand.Add(display);
        UpdateVisuals();
    }

    public void ShuffleDrawPile()
    {
        for (int i = DrawPile.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            CardData temp = DrawPile[i];
            DrawPile[i] = DrawPile[j];
            DrawPile[j] = temp;
        }
    }

    public void ResetDiscardPile()
    {
        if (DiscardPile.Count > 0)
        {
            DrawPile.AddRange(DiscardPile);
            DiscardPile.Clear();
        }
    }

    public void SetHandSize(int size)
    {
        m_HandSize = size;
    }

    public void OnAddNewCardToDeck(CardData newCard)
    {
        Deck.Add(newCard);
    }

    public void RemoveCardFromDeck(CardData removed)
    {
        Deck.Remove(removed);
    }

    public void DrawHand()
    {
        for (int i = 0; i < m_HandSize; i++)
        {
            DrawCard();
        }
        _player.UpdateGeneralUI();
    }

    public void DiscardHand()
    {
        while (Hand.Count > 0)
        {
            var card = Hand[0];
            Hand.Remove(card);
            DiscardPile.Add(card._cardData);
            Destroy(card.gameObject);
        }
        _player.UpdateGeneralUI();
    }

    public void UpdateVisuals()
    {
        if (Hand.Count == 1)
        {

            Hand[0].transform.localPosition = Vector3.zero;
            Hand[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        for (int i = 0; i < Hand.Count; i++)
        {
            float rotationAngle = (m_FanSpread * (i - (Hand.Count - 1) / 2f));
            Hand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            float horizontalOffset = (m_HorizontalSpacing * (i - (Hand.Count - 1) / 2f));
            float normalizedPosition = (2f * i / (Hand.Count - 1) - 1f);
            float verticalOffset = Mathf.Max(0.01f, m_VerticalSpacing * (1 - normalizedPosition * normalizedPosition));
            Hand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);

        }
    }

    public void PlayCard(CardDisplay card)
    {
        Hand.Remove(card);
        DiscardPile.Add(card._cardData);
        Destroy(card.gameObject);
        UpdateVisuals();
    }
}

