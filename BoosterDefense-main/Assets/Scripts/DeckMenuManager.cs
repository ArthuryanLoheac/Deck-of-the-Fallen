using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

public enum DeckEmplacement {
    All,
    Deck
}

public class DeckMenuManager : MonoBehaviour
{
    public static DeckMenuManager instance;
    [Header("Cards")]
    public GameObject cardZoomed;
    public GameObject cardPrefabDeck;
    [Header("Canvas & Scroll")]
    public GameObject canvasAllCards;
    public GameObject canvasDeckCards;
    public GameObject scrollDeckCards;
    public GameObject canvasCardMoved;
    [Header("Titles")]
    public GameObject titleDeckCards;
    public TMP_Text nbDeckCards;
    public TMP_Text nbAllCards;

    [Header("Offsets")]
    public Vector2 offsetAll = new Vector2(0, 0);
    public Vector2 offsetDeck = new Vector2(0, 0);
    public Vector2 offsetBetweenCards = new Vector2(0, 0);
    [Header("Others")]
    [HideInInspector] public List<List<GameObject>> cardsAll = new List<List<GameObject>>();
    [HideInInspector] public List<List<GameObject>> cardsDeck = new List<List<GameObject>>();
    Vector2 sizeCard;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Vector2 rawSizeCard = new Vector2(cardPrefabDeck.GetComponent<RectTransform>().sizeDelta.x,
            cardPrefabDeck.GetComponent<RectTransform>().sizeDelta.y);
        sizeCard = rawSizeCard * cardPrefabDeck.transform.localScale.x;

        cardZoomed.SetActive(false);
        initStock();
        initDeck();
        SetSizeCanva();
        RefreshAll();
    }
    private GameObject RemoveCardX(int i, int x, List<List<GameObject>> lstCards)
    {
        GameObject a = lstCards[i][x];
        lstCards[i].RemoveAt(x);
        return a;
    }

    GameObject InitCardAllRaw(CardStats cardStat, GameObject canvas)
    {
        GameObject card = Instantiate(cardPrefabDeck, canvas.transform);
        card.GetComponent<Card>().SetStats(cardStat);
        return card;
    }

    private void updateNbCards()
    {
        nbDeckCards.text = DeckCardsManager.instance.deck.Count.ToString();
        nbAllCards.text = DeckCardsManager.instance.AllCards.Count.ToString();
    }

    public void RefreshAll()
    {
        UpdatePosCardsAll();
        UpdatePosCardsDeck();
        updateNbCards();
    }

    void UpdateOnlyNecessaryList(bool isAll)
    {
        UpdatePosCardsAll();
        UpdatePosCardsDeck();
    }

    public void AddCardTo(bool All, GameObject card)
    {
        List<List<GameObject>> lstCards;
        GameObject canvas;

        if (All)
        {
            DeckCardsManager.instance.AllCards.Add(card.GetComponent<Card>().cardStats);
            lstCards = cardsAll;
            canvas = canvasAllCards;
        }
        else
        {
            DeckCardsManager.instance.deck.Add(card.GetComponent<Card>().cardStats);
            lstCards = cardsDeck;
            canvas = canvasDeckCards;
        }
        updateNbCards();

        for (int i = 0; i < lstCards.Count; i++)
        {
            if (lstCards[i].Count > 0 && lstCards[i][0].GetComponent<Card>().cardStats == card.GetComponent<Card>().cardStats)
            {
                lstCards[i][0].GetComponent<Card>().cardCount += 1;
                if (lstCards[i].Count >= 3)
                {
                    Destroy(card);
                }
                else
                {
                    lstCards[i].Add(card);
                    card.transform.SetParent(canvas.transform);
                    card.transform.SetSiblingIndex(1);
                }
                UpdateOnlyNecessaryList(All);
                return;
            }
        }
        // Not existing
        lstCards.Add(new List<GameObject> { card });
        card.transform.SetParent(canvas.transform);
        UpdateOnlyNecessaryList(All);
    }

    public GameObject TakeCardFrom(Card card)
    {
        bool All = card.transform.parent.gameObject == canvasAllCards;
        List<List<GameObject>> lstCards = All ? cardsAll : cardsDeck;
        GameObject canvas = All ? canvasAllCards : canvasDeckCards;

        if (All)
            DeckCardsManager.instance.AllCards.Remove(card.cardStats);
        else
        {
            DeckCardsManager.instance.deck.Remove(card.cardStats);
        }
        updateNbCards();

        for (int i = 0; i < lstCards.Count; i++)
        {
            if (lstCards[i].Count > 0 && lstCards[i][0].GetComponent<Card>().cardStats == card.cardStats)
            {
                GameObject a;
                if (lstCards[i].Count == 1)
                {
                    // Last Card
                    a = RemoveCardX(i, 0, lstCards);
                    lstCards.RemoveAt(i);
                }
                else
                {
                    // Remain at least 1 card after
                    lstCards[i][1].GetComponent<Card>().cardCount = lstCards[i][0].GetComponent<Card>().cardCount - 1;
                    lstCards[i][0].GetComponent<Card>().cardCount = 1;
                    a = RemoveCardX(i, 0, lstCards);
                    if (lstCards[i][0].GetComponent<Card>().cardCount > 2)
                    {
                        GameObject newCard = InitCardAllRaw(lstCards[i][0].GetComponent<Card>().cardStats, canvas);
                        newCard.transform.SetSiblingIndex(1);
                        lstCards[i].Add(newCard);
                    }
                }
                UpdateOnlyNecessaryList(All);
                return a;
            }
        }
        return null;
    }

    private void UpdatePosCardsAll()
    {
        SetSizeCanva();
        Vector2 sizeCanvas = new Vector2(canvasAllCards.GetComponent<RectTransform>().sizeDelta.x,
            canvasAllCards.GetComponent<RectTransform>().sizeDelta.y);

        cardsAll.Sort(delegate (List<GameObject> a, List<GameObject> b)
        {
            return a[0].GetComponent<Card>().cardStats.name.CompareTo(b[0].GetComponent<Card>().cardStats.name);
        });

        for (int i = 0; i < cardsAll.Count; i++)
        {
            for (int j = 0; j < cardsAll[i].Count; j++)
            {
                cardsAll[i][j].transform.localPosition = new Vector3(-sizeCanvas.x / 2 + (i / 2 * (sizeCard.x + offsetBetweenCards.x)) + (sizeCard.x / 2f) + offsetAll.x,
                    sizeCanvas.y / 2 - sizeCard.y / 2f - offsetAll.y - (i % 2 == 1 ? sizeCard.y + offsetBetweenCards.y : 0) - j * 10,
                    0);
            }
        }
    }
    private void UpdatePosCardsDeck()
    {
        SetSizeCanva();
        Vector2 sizeCanvas = new Vector2(canvasDeckCards.GetComponent<RectTransform>().sizeDelta.x,
            canvasDeckCards.GetComponent<RectTransform>().sizeDelta.y);

        cardsDeck.Sort(delegate (List<GameObject> a, List<GameObject> b)
        {
            return a[0].GetComponent<Card>().cardStats.name.CompareTo(b[0].GetComponent<Card>().cardStats.name);
        });

        for (int i = 0; i < cardsDeck.Count; i++)
        {
            for (int j = 0; j < cardsDeck[i].Count; j++)
            {
                cardsDeck[i][j].transform.localPosition = new Vector3(-sizeCanvas.x / 2 + (i * (sizeCard.x + offsetBetweenCards.x)) + (sizeCard.x / 2f) + offsetDeck.x,
                sizeCanvas.y / 2 - sizeCard.y / 2f - offsetDeck.y - j * 10,
                0);
            }
        }
    }


    void SetSizeCanva()
    {
        canvasDeckCards.GetComponent<RectTransform>().sizeDelta = new Vector2(
                                    Mathf.Max(1400, cardsDeck.Count * (sizeCard.x + offsetBetweenCards.x) + offsetDeck.x),
                                    canvasDeckCards.GetComponent<RectTransform>().sizeDelta.y);
        canvasAllCards.GetComponent<RectTransform>().sizeDelta = new Vector2(
                                    Mathf.Max(1400, Mathf.Round((cardsAll.Count + 1) / 2) * (sizeCard.x + offsetBetweenCards.x) + offsetAll.x),
                                    canvasAllCards.GetComponent<RectTransform>().sizeDelta.y);
    }



    int getJPos(CardStats stat, List<List<GameObject>> lst)
    {
        for (int i = 0; i < lst.Count; i++)
        {
            if (stat == lst[i][0].GetComponent<Card>().cardStats)
            {
                return i;
            }
        }
        return -1;
    }

    int HandleCardExisting(int i, List<List<GameObject>> cards, List<CardStats> instanceCards)
    {
        for (int j = 0; j < cards.Count; j++)
        {
            if (cards[j][0].GetComponent<Card>().cardStats.name == instanceCards[i].name &&
                cards[j][0].GetComponent<Card>().cardStats.artType == instanceCards[i].artType)
            {
                cards[j][0].GetComponent<Card>().cardCount += 1;
                return cards[j][0].GetComponent<Card>().cardCount;
            }
        }
        return 0;
    }

    GameObject InitCard(int i, GameObject canvas, List<CardStats> lst)
    {
        GameObject card = Instantiate(cardPrefabDeck, canvas.transform);
        card.GetComponent<Card>().SetStats(lst[i]);
        return card;
    }

    #region "DECK"

    int initDeck()
    {
        int iPos = 0;

        DeckCardsManager.instance.deck.Sort(delegate (CardStats a, CardStats b)
        {
            int nameComparison = a.name.CompareTo(b.name);
            if (nameComparison != 0)
                return nameComparison;
            return a.artType.CompareTo(b.artType);
        });

        for (int i = 0; i < DeckCardsManager.instance.deck.Count; i++)
        {
            int nbCard = HandleCardExisting(i, cardsDeck, DeckCardsManager.instance.deck);
            if (nbCard > 3)
                continue;
            if (nbCard == 0)
            {
                GameObject card = InitCard(i, canvasDeckCards, DeckCardsManager.instance.deck);
                cardsDeck.Add(new List<GameObject> { card });
                iPos++;
            }
            else
            {
                int jPos = getJPos(DeckCardsManager.instance.deck[i], cardsDeck);
                GameObject card = InitCard(i, canvasDeckCards, DeckCardsManager.instance.deck);
                card.transform.SetSiblingIndex(1);
                cardsDeck[jPos].Add(card);
            }
        }
        return iPos;
    }

    #endregion "DECK"

    #region "STOCK"

    int initStock()
    {
        int iPos = 0;

        DeckCardsManager.instance.AllCards.Sort(delegate (CardStats a, CardStats b)
        {
            return a.name.CompareTo(b.name);
        });

        for (int i = 0; i < DeckCardsManager.instance.AllCards.Count; i++)
        {
            int nbCard = HandleCardExisting(i, cardsAll, DeckCardsManager.instance.AllCards);
            if (nbCard > 3)
                continue;
            if (nbCard == 0)
            {
                GameObject card;
                card = InitCard(i, canvasAllCards, DeckCardsManager.instance.AllCards);
                cardsAll.Add(new List<GameObject> { card });
                iPos++;
            }
            else
            {
                int jPos = getJPos(DeckCardsManager.instance.AllCards[i], cardsAll);
                GameObject card = InitCard(i, canvasAllCards, DeckCardsManager.instance.AllCards);
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

        if (!zoomed.cardStats || stats.cardStats.name != zoomed.cardStats.name)
        {
            zoomed.SetStats(stats.cardStats);
            cardZoomed.SetActive(true);
        }
        else
        {
            cardZoomed.SetActive(false);
            zoomed.cardStats = null;
        }
    }
    #endregion "ZOOMED"
}
