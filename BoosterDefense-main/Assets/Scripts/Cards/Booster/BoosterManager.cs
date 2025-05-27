using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    public static BoosterManager instance;
    public Animation animationNormalDraw;
    private float[] chanceFullArtList = {0.03f, 0.05f, 0.08f};

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
        double chance = Random.Range(0.0f, 1.0f);
        if (chance <= chanceFullArtList[rareLevel])
            cardDraw.artType = TypeCardArt.FULL_ART;
        if (AddCardToHand)
                CardsManager.instance.AddCard(cardDraw);

        DeckCardsManager.instance.AllCards.Add(cardDraw);
        return cardDraw;
    }

    IEnumerator DrawAnimationCoroutine(BoosterStats boosterStats, bool AddCardToHand = false)
    {
        BoosterDrawCardUI.instance.DesactiveCard();
        List<CardStats> cards = new List<CardStats>();
        List<int> raritys = new List<int>();
        // Comon
        BoosterDrawCardUI.instance.isDrawing = true;
        for (int i = 0; i < boosterStats.nbCard - boosterStats.nbRare; i++)
        {
            cards.Add(DrawFromList(boosterStats.listCardCommon, 0, AddCardToHand));
            raritys.Add(0);
        }
        //Rare and super rare
        for (int j = 0; j < boosterStats.nbRare; j++) {
            if (Random.Range(1, 101) >= boosterStats.percentSuperRare) {
                //rare
                cards.Add(DrawFromList(boosterStats.listCardRare, 1, AddCardToHand));
                raritys.Add(1);
            } else {
                //Super rare
                cards.Add(DrawFromList(boosterStats.listCardSuperRare, 2, AddCardToHand));
                raritys.Add(2);
            }
        }
        BoosterDrawCardUI.instance.SetupCards(cards, raritys);
    
        yield return new WaitForSeconds(30);
        BoosterDrawCardUI.instance.DesactiveCard();
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
