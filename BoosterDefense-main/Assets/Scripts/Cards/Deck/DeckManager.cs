using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;
    public int cardsDrawned;
    public Button drawButton;
    public List<CardStats> cards;
    public List<CardStats> cardsUsed;
    public List<CardStats> StartHand;

    private void CheckSwapCardsUsedToCards()
    {
        if (cards.Count <= 0) {
            foreach(CardStats card in cardsUsed) {
                cards.Add(card);
            }
            cardsUsed = new List<CardStats>();
        }
    }

    void Update()
    {
        CheckSwapCardsUsedToCards();
    }

    public void GiveCardStart()
    {
        //donne les cartes de la main dans la main
        foreach(CardStats cardstat in StartHand) {
            CardsManager.instance.AddCard(cardstat);
        }
    }

    public void AddCardUsed(CardStats card)
    {
        cardsUsed.Add(card);
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //Ajoute carte du deck dans le deck
        foreach (CardStats card in DeckCardsManager.instance.deck)
            cards.Add(card);
        ShuffleDeck();
    }

    public void AddCardDeckEnd(CardStats card)
    {
        cards.Add(card);
    }
    public void AddCardDeckStart(CardStats card)
    {
        cards.Insert(0, card);
    }
    public void AddCardDeckEndAndShuffle(CardStats card)
    {
        AddCardDeckEnd(card);
        ShuffleDeck();
    }
    public void AddCardDeckStartAndShuffle(CardStats card)
    {
        AddCardDeckStart(card);
        ShuffleDeck();
    }

    public void ShuffleDeck()
    {
        List<CardStats> copy = new List<CardStats>();
        int sizeDeck = cards.Count;

        for (int i = 0; i < sizeDeck; i++) {
            int id = Random.Range(0, cards.Count);
            copy.Add(cards[id]);
            cards.RemoveAt(id);
        }
        cards = copy;
    }

    public List<CardStats> DrawXCard(int x)
    {
        List<CardStats> cardsDrawned = new List<CardStats>();
        int nbCardDraw = x;
        if (cards.Count < x) nbCardDraw = cards.Count;
        for (int i = 0; i < nbCardDraw; i++)
            cardsDrawned.Add(cards[i]);
        cards.RemoveRange(0, nbCardDraw);
        return cardsDrawned;
    }

    public void DrawAndSpawnXCard(int x)
    {
        List<CardStats> list = DrawXCard(x);
        foreach(CardStats card in list) {
            CardsManager.instance.AddCard(card);
        }
    }

    public void DrawAndSpawnCardValue()
    {
        StartCoroutine(playSoundDraw(cardsDrawned));
        for (int i = 0; i < cardsDrawned; i++) {
            DrawAndSpawnXCard(1);
            CheckSwapCardsUsedToCards();
        }
    }
    IEnumerator playSoundDraw(int i)
    {
        for (int j = 0; j < i; j++) {
            SoundManager.instance.PlaySound("DrawCard");
            yield return new WaitForSeconds(0.2f);
        }
    }
}
