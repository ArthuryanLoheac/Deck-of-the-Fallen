using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckMenuManager : MonoBehaviour
{
    public static DeckMenuManager instance;
    public GameObject cardZoomed;
    public GameObject cardPrefabDeck;
    public GameObject canvasAllCards;
    public Vector2 offset = new Vector2(0, 0);
    public Vector2 offsetBetweenCards = new Vector2(0, 0);
    private List<GameObject> cardsAll = new List<GameObject>();

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    bool HandleCardExisting(int i)
    {
        for (int j = 0; j < cardsAll.Count; j++) {
            if (cardsAll[j].GetComponent<Card>().cardStats.name == DeckCardsManager.instance.AllCards[i].name) {
                cardsAll[j].GetComponent<Card>().cardCount += 1;
                return true;
            }
        }
        return false;
    }

    void Start()
    {
        cardZoomed.SetActive(false);
        int iPos = 0;

        Vector2 sizeCanvas = new Vector2(canvasAllCards.GetComponent<RectTransform>().sizeDelta.x,
            canvasAllCards.GetComponent<RectTransform>().sizeDelta.y);
        Vector2 sizeCard = new Vector2(cardPrefabDeck.GetComponent<RectTransform>().sizeDelta.x,
            cardPrefabDeck.GetComponent<RectTransform>().sizeDelta.y) * cardPrefabDeck.transform.localScale.x;

        for (int i = 0; i < DeckCardsManager.instance.AllCards.Count; i++) {
            if (HandleCardExisting(i))
                continue;
            GameObject card = Instantiate(cardPrefabDeck, canvasAllCards.transform);
            card.transform.localPosition = new Vector3(-sizeCanvas.x/2 + (iPos/2 * (sizeCard.x + offsetBetweenCards.x)) + (sizeCard.x/2f) + offset.x,
                sizeCanvas.y/2 - sizeCard.y /2f - offset.y - (iPos % 2 == 0 ? sizeCard.y + offsetBetweenCards.y : 0),
                0);
            card.GetComponent<Card>().SetStats(DeckCardsManager.instance.AllCards[i]);
            cardsAll.Add(card);
            iPos++;
        }
    }

    public void SetStatsZommed(Card stats)
    {
        Card zoomed = cardZoomed.GetComponent<Card>();

        if (!zoomed.cardStats || stats.cardStats.name != zoomed.cardStats.name) {
            zoomed.SetStats(stats.cardStats);
            cardZoomed.SetActive(true);
        } else {
            cardZoomed.SetActive(false);
            zoomed.cardStats = null;
        }
    }
}
