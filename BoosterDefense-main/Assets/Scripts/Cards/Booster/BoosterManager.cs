using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    public static BoosterManager instance;
    public Animation animationNormalDraw;
    private float[] chanceFullArtList = {0.01f, 0.05f, 0.05f};

    public List<BoosterStats> boosterOwned;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }


    private CardStats DrawFromList(List<CardStats> lst, int rareLevel = 0, bool AddCardToHand = false)
    {
        //Random Carte
        CardStats cardDraw = Instantiate(lst[Random.Range(0, lst.Count)]);
        if (Random.Range(0.0f, 1.0f) <= chanceFullArtList[rareLevel])
            cardDraw.artType = TypeCardArt.FULL_ART;
        if (AddCardToHand)
                CardsManager.instance.AddCard(cardDraw);

        DeckCardsManager.instance.AllCards.Add(cardDraw);
        return cardDraw;
    }

    IEnumerator DrawAnimationCoroutine(BoosterStats boosterStats, bool AddCardToHand = false)
    {
        // Comon
        BoosterDrawCardUI.instance.isDrawing = true;
        for (int i = 0; i < boosterStats.nbCard - boosterStats.nbRare; i++) {
            BoosterDrawCardUI.instance.DesactiveCard();

            CardStats card = DrawFromList(boosterStats.listCardCommon, 0, AddCardToHand);

            BoosterDrawCardUI.instance.SetupCard(card, 0);
            SoundManager.instance.PlaySound("DrawCard");
            yield return new WaitForSeconds(1.5f);
        }
        //Rare and super rare
        for (int j = 0; j < boosterStats.nbRare; j++) {
            BoosterDrawCardUI.instance.DesactiveCard();

            if (Random.Range(1, 101) < boosterStats.percentSuperRare) {
                //Super rare
                CardStats card = DrawFromList(boosterStats.listCardSuperRare, 2, AddCardToHand);
                BoosterDrawCardUI.instance.SetupCard(card, 1);
                SoundManager.instance.PlaySound("DrawCard");
                yield return new WaitForSeconds(1.5f);
            } else {
                //rare
                CardStats card = DrawFromList(boosterStats.listCardRare, 1, AddCardToHand);
                BoosterDrawCardUI.instance.SetupCard(card, 1);
                SoundManager.instance.PlaySound("DrawCard");
                yield return new WaitForSeconds(1.5f);
            }
            BoosterDrawCardUI.instance.DesactiveCard();
        }
        BoosterDrawCardUI.instance.isDrawing = false;
        //Fermeture marchand apres pioche
        if (BoosterMarchandManager.instance) {
            BoosterMarchandManager.instance.Activated = false;
            TimerCoolDown.instance.setIconWait(IconWaveType.Wait);
            WavesManager.instance.nextTypeWave();
        }
    }
    public void OpenBooster(BoosterStats boosterStats, bool AddCardToHand = false)
    {
        StartCoroutine(DrawAnimationCoroutine(boosterStats, AddCardToHand));
    }
}
