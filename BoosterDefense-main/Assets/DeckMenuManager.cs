using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class DeckMenuManager : MonoBehaviour
{
    public static DeckMenuManager instance;
    public GameObject cardZoomed;
    public GameObject cardPrefabDeck;
    public GameObject canvasAllCards;
    public GameObject canvasDeckCards;
    public Vector2 offsetAll = new Vector2(0, 0);
    public Vector2 offsetDeck = new Vector2(0, 0);
    public Vector2 offsetBetweenCards = new Vector2(0, 0);
    private List<List<GameObject>> cardsAll = new List<List<GameObject>>();
    private List<List<GameObject>> cardsDeck = new List<List<GameObject>>();
    Vector2 sizeCard;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Reset()
    {
        for (int i = 0; i < cardsAll.Count; i++) {
            for (int j = 0; j < cardsAll[i].Count; j++) {
                Destroy(cardsAll[i][j]);
            }
        }
        cardsAll.Clear();
        for (int i = 0; i < cardsDeck.Count; i++) {
            for (int j = 0; j < cardsDeck[i].Count; j++) {
                Destroy(cardsDeck[i][j]);
            }
        }
        cardsDeck.Clear();
    }

    void Start()
    {
        Vector2 rawSizeCard = new Vector2(cardPrefabDeck.GetComponent<RectTransform>().sizeDelta.x,
            cardPrefabDeck.GetComponent<RectTransform>().sizeDelta.y);
        sizeCard = rawSizeCard * cardPrefabDeck.transform.localScale.x;
        cardZoomed.SetActive(false);
        initStock();
        initDeck();
        canvasDeckCards.GetComponent<RectTransform>().sizeDelta = new Vector2(
                                    cardsDeck.Count * (sizeCard.x + offsetBetweenCards.x) + offsetDeck.x,
                                    canvasDeckCards.GetComponent<RectTransform>().sizeDelta.y);
        canvasAllCards.GetComponent<RectTransform>().sizeDelta = new Vector2(
                                    cardsAll.Count/2 * (sizeCard.x + offsetBetweenCards.x) + offsetAll.x,
                                    canvasAllCards.GetComponent<RectTransform>().sizeDelta.y);
        Reset();
        initStock();
        initDeck();
    }

    #region "DECK"

    int getJPosDeck(CardStats stat)
    {
        for (int i = 0; i < cardsDeck.Count; i++) {
            if (stat.name == cardsDeck[i][0].GetComponent<Card>().cardStats.name) {
                return i;
            }
        }
        return -1;
    }

    int HandleCardExistingDeck(int i)
    {
        for (int j = 0; j < cardsDeck.Count; j++) {
            if (cardsDeck[j][0].GetComponent<Card>().cardStats.name == DeckCardsManager.instance.deck[i].name) {
                cardsDeck[j][0].GetComponent<Card>().cardCount += 1;
                return cardsDeck[j][0].GetComponent<Card>().cardCount;
            }
        }
        return 0;
    }

    GameObject InitCardDeck(Vector2 sizeCanvas, int iPos, int i, int depth)
    {
        GameObject card = Instantiate(cardPrefabDeck, canvasDeckCards.transform);
        card.transform.localPosition = new Vector3(-sizeCanvas.x/2 + (iPos * (sizeCard.x + offsetBetweenCards.x)) + (sizeCard.x/2f) + offsetDeck.x,
            sizeCanvas.y/2 - sizeCard.y /2f - offsetDeck.y - depth*10,
            0);
        card.GetComponent<Card>().SetStats(DeckCardsManager.instance.deck[i]);
        return card;
    }

    int initDeck()
    {
        int iPos = 0;

        Vector2 sizeCanvas = new Vector2(canvasDeckCards.GetComponent<RectTransform>().sizeDelta.x,
            canvasDeckCards.GetComponent<RectTransform>().sizeDelta.y);

        for (int i = 0; i < DeckCardsManager.instance.deck.Count; i++) {
            int nbCard = HandleCardExistingDeck(i);
            if (nbCard > 3)
                continue;
            if (nbCard == 0) {
                GameObject card = InitCardDeck(sizeCanvas, iPos, i, 0);
                cardsDeck.Add(new List<GameObject> { card });
                iPos++;
            } else {
                int jPos = getJPosDeck(DeckCardsManager.instance.deck[i]);
                GameObject card = InitCardDeck(sizeCanvas, jPos, i, nbCard-1);
                card.transform.SetSiblingIndex(1);
                cardsDeck[jPos].Add(card);
            }
        }
        return iPos;
    }

    #endregion "DECK"

    #region "STOCK"

    int HandleCardExistingAll(int i)
    {
        for (int j = 0; j < cardsAll.Count; j++) {
            if (cardsAll[j][0].GetComponent<Card>().cardStats.name == DeckCardsManager.instance.AllCards[i].name) {
                cardsAll[j][0].GetComponent<Card>().cardCount += 1;
                return cardsAll[j][0].GetComponent<Card>().cardCount;
            }
        }
        return 0;
    }

    int getJPosAll(CardStats stat)
    {
        for (int i = 0; i < cardsAll.Count; i++) {
            if (stat.name == cardsAll[i][0].GetComponent<Card>().cardStats.name) {
                return i;
            }
        }
        return -1;
    }

    GameObject InitCardAll(Vector2 sizeCanvas, int iPos, int i, int depth)
    {
        GameObject card = Instantiate(cardPrefabDeck, canvasAllCards.transform);
        card.transform.localPosition = new Vector3(-sizeCanvas.x/2 + (iPos/2 * (sizeCard.x + offsetBetweenCards.x)) + (sizeCard.x/2f) + offsetAll.x,
            sizeCanvas.y/2 - sizeCard.y /2f - offsetAll.y - (iPos % 2 == 0 ? sizeCard.y + offsetBetweenCards.y : 0) - depth*10,
            0);
        card.GetComponent<Card>().SetStats(DeckCardsManager.instance.AllCards[i]);
        return card;
    }

    int initStock()
    {
        int iPos = 0;

        Vector2 sizeCanvas = new Vector2(canvasAllCards.GetComponent<RectTransform>().sizeDelta.x,
            canvasAllCards.GetComponent<RectTransform>().sizeDelta.y);

        for (int i = 0; i < DeckCardsManager.instance.AllCards.Count; i++) {
            int nbCard = HandleCardExistingAll(i);
            if (nbCard > 3)
                continue;
            if (nbCard == 0) {
                GameObject card;
                card = InitCardAll(sizeCanvas, iPos, i, 0);
                cardsAll.Add(new List<GameObject> { card });
                iPos++;
            } else {
                int jPos = getJPosAll(DeckCardsManager.instance.AllCards[i]);
                GameObject card = InitCardAll(sizeCanvas, jPos, i, nbCard-1);
                card.transform.SetSiblingIndex(1);
                cardsAll[jPos].Add(card);
            }
        }
        return iPos;
    }

    #endregion "STOCK"

    #region "ZOOMED"
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
    #endregion "ZOOMED"
}
