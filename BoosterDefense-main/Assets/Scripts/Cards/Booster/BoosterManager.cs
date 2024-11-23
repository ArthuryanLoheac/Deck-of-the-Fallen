using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    public static BoosterManager instance;
    public Animation animationNormalDraw;

    public List<BoosterStats> boosterOwned;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }


    private CardStats DrawFromList(List<CardStats> lst, bool AddCardToHand = false)
    {
        //Random Carte
        CardStats cardDraw = lst[Random.Range(0, lst.Count)];
        if (AddCardToHand) {
            //Ajoute carte a la main
            CardsManager.instance.AddCard(cardDraw);
        } else {
            //Ajoute dans le deck
            DeckCardsManager.instance.AllCards.Add(cardDraw);
        }
        return cardDraw;
    }

    IEnumerator DrawAnimationCoroutine(BoosterStats boosterStats, bool AddCardToHand = false)
    {
        BoosterDrawCardUI.instance.isDrawing = true;
        for (int i = 0; i < boosterStats.nbCard - boosterStats.nbRare; i++) {
            BoosterDrawCardUI.instance.DesactiveCard();

            CardStats card = DrawFromList(boosterStats.listCardCommon, AddCardToHand);

            BoosterDrawCardUI.instance.SetupCard(card, 0);
            yield return new WaitForSeconds(1.5f);
        }
        //Rare and super rare
        for (int j = 0; j < boosterStats.nbRare; j++) {
            BoosterDrawCardUI.instance.DesactiveCard();

            if (Random.Range(1, 101) < boosterStats.percentSuperRare) {
                //Super rare
                CardStats card = DrawFromList(boosterStats.listCardSuperRare, AddCardToHand);
                BoosterDrawCardUI.instance.SetupCard(card, 1);
            yield return new WaitForSeconds(1.5f);
            } else {
                //rare
                CardStats card = DrawFromList(boosterStats.listCardRare, AddCardToHand);
                BoosterDrawCardUI.instance.SetupCard(card, 1);
                yield return new WaitForSeconds(1.5f);
            }
            BoosterDrawCardUI.instance.DesactiveCard();
        }
        BoosterDrawCardUI.instance.isDrawing = false;
        if (BoosterMarchandManager.instance)
            BoosterMarchandManager.instance.Activated = false;
    }
    public void OpenBooster(BoosterStats boosterStats, bool AddCardToHand = false)
    {
        StartCoroutine(DrawAnimationCoroutine(boosterStats, AddCardToHand));
    }
}
