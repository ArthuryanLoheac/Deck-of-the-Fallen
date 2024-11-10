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


    private void DrawFromList(List<CardStats> lst, bool AddCardToHand = false)
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
    }

    public void OpenBooster(BoosterStats boosterStats, bool AddCardToHand = false)
    {
        //Draw Normal Cards
        for (int i = 0; i < boosterStats.nbCard - boosterStats.nbRare; i++) {
            DrawFromList(boosterStats.listCardCommon, AddCardToHand);
        }

        //Rare and super rare
        for (int j = 0; j < boosterStats.nbRare; j++) {
            if (Random.Range(1, 101) < boosterStats.percentSuperRare) {
                //Super rare
                DrawFromList(boosterStats.listCardSuperRare, AddCardToHand);
            } else {
                //rare
                DrawFromList(boosterStats.listCardRare, AddCardToHand);
            }
        }
    }
}
