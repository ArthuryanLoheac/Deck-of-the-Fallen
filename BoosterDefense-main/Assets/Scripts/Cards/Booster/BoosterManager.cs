using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    public static BoosterManager instance;

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
        for (int i = 0; i < boosterStats.nbCard - boosterStats.nbRare; i++) {
            BoosterDrawCardUI.instance.DesactiveCard();
            yield return new WaitForSeconds(0.5f);

            CardStats card = DrawFromList(boosterStats.listCardCommon, AddCardToHand);

            BoosterDrawCardUI.instance.SetupCard(card);
            yield return new WaitForSeconds(1.5f);
        }
        //Rare and super rare
        for (int j = 0; j < boosterStats.nbRare; j++) {
            BoosterDrawCardUI.instance.DesactiveCard();

            if (Random.Range(1, 101) < boosterStats.percentSuperRare) {
                //Super rare
                yield return new WaitForSeconds(2f);
                CardStats card = DrawFromList(boosterStats.listCardSuperRare, AddCardToHand);
                BoosterDrawCardUI.instance.SetupCard(card);
            } else {
                //rare
                yield return new WaitForSeconds(2f);
                CardStats card = DrawFromList(boosterStats.listCardRare, AddCardToHand);
                BoosterDrawCardUI.instance.SetupCard(card);
            }
            yield return new WaitForSeconds(1f);
            BoosterDrawCardUI.instance.DesactiveCard();
        }
    }
    public void OpenBooster(BoosterStats boosterStats, bool AddCardToHand = false)
    {
        StartCoroutine(DrawAnimationCoroutine(boosterStats, AddCardToHand));
    }
}
