using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeckEmplacement {
    All,
    Deck,
    Hand
}

public class DeckUIManagement : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject prefabUICardPlacement;
    public GameObject prefabUICardPlacementHand;
    public GameObject cardPrefab;
    [Header("Canvas")]
    public Transform canvasAll;
    public Transform canvasDeck;
    public Transform canvasHand;
    float offsetAll = 0;
    float offsetDeck = 0;
    private List<GameObject> cardsBackAll;
    private List<GameObject> cardsBackDeck;
    private List<GameObject> cardsBackHand;
    [Header("Speed Mouse Wheel")]
    public float speedMouseWheel = 50f;

    public static DeckUIManagement instance;
    [Header("Size cards & offset")]
    public Vector2 sizeCard = new Vector2(165, 220);
    public Vector2 offSetPos = new Vector2(130, 310);

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Refresh()
    {
        foreach(GameObject obj in cardsBackAll) {
            Destroy(obj);
        }
        foreach(GameObject obj in cardsBackDeck) {
            Destroy(obj);
        }
        foreach(GameObject obj in cardsBackHand) {
            Destroy(obj);
        }
        InitAlls();
        InitHand();
        InitDeck();
        InitAllsCards();
        InitCardsDeck();
        InitCardsHand();

    }

    void Start()
    {
        offsetAll = 0;
        offsetDeck = 0;
        InitAlls();
        InitHand();
        InitDeck();
        InitAllsCards();
        InitCardsDeck();
        InitCardsHand();
    }

    #region cards

    private void InitAllsCards()
    {
        int i = 0;

        foreach(CardStats card in DeckCardsManager.instance.AllCards) {
            GameObject cardCreate = Instantiate(cardPrefab, cardsBackAll[i].transform);
            cardCreate.GetComponent<Card>().SetStats(card);
            cardCreate.GetComponent<DraggableCardDeck>().emplacement = DeckEmplacement.All;
            i++;
        }
    }
    
    private void InitCardsDeck() 
    {
        int i = 0;

        foreach(CardStats card in DeckCardsManager.instance.deck) {
            GameObject cardCreate = Instantiate(cardPrefab, cardsBackDeck[i].transform);
            cardCreate.GetComponent<Card>().SetStats(card);
            cardCreate.GetComponent<DraggableCardDeck>().emplacement = DeckEmplacement.Deck;
            i++;
        }
    }

    private void InitCardsHand()
    {
        int i = 0;

        foreach(CardStats card in DeckCardsManager.instance.StartHand) {
            GameObject cardCreate = Instantiate(cardPrefab, cardsBackHand[i].transform);
            cardCreate.GetComponent<Card>().SetStats(card);
            cardCreate.GetComponent<DraggableCardDeck>().emplacement = DeckEmplacement.Hand;
            i++;
        }
    }

    #endregion cards

    #region BackCards
    
    private void InitAlls()
    {
        int nbCard = (int)Mathf.Round(DeckCardsManager.instance.AllCards.Count / 5) * 5 + 5;
        nbCard = Mathf.Max(20, nbCard);
        cardsBackAll = new List<GameObject>();
        for (int i = 0; i < nbCard; i++) {
            Vector3 pos = new Vector3((int)(i % 5) * sizeCard.x + offSetPos.x, (i / 5) * -sizeCard.y + offSetPos.y, 0);
            GameObject obj = Instantiate(prefabUICardPlacement, canvasAll);
            obj.GetComponent<RectTransform>().anchoredPosition = pos;
            cardsBackAll.Add(obj);
        }
    }
    private void InitHand()
    {
        cardsBackHand = new List<GameObject>();
        for (int i = 0; i < 5; i++) {
            Vector3 pos = new Vector3((int)(i % 5) * sizeCard.x + offSetPos.x, (i / 5) * -sizeCard.y + offSetPos.y, 0);
            GameObject obj = Instantiate(prefabUICardPlacementHand, canvasHand);
            obj.GetComponent<RectTransform>().anchoredPosition = pos;
            cardsBackHand.Add(obj);
        }
    }
    private void InitDeck()
    {
        int nbCard = (int)Mathf.Round(DeckCardsManager.instance.deck.Count / 5) * 5 + 5;
        nbCard = Mathf.Max(20, nbCard);
        cardsBackDeck = new List<GameObject>();
        for (int i = 0; i < nbCard; i++) {
            Vector3 pos = new Vector3((int)(i % 5) * sizeCard.x + offSetPos.x, (i / 5) * -sizeCard.y + (offSetPos.y -sizeCard.y), 0);
            GameObject obj = Instantiate(prefabUICardPlacement, canvasDeck);
            obj.GetComponent<RectTransform>().anchoredPosition = pos;
            cardsBackDeck.Add(obj);
        }
    }

    private void SetPosWithOffset()
    {
        int i = 0;
        foreach(GameObject obj in cardsBackAll) {
            Vector3 pos = new Vector3((i % 5) * sizeCard.x + offSetPos.x, (int)(i / 5) * -sizeCard.y + offSetPos.y + offsetAll, 0);
            obj.GetComponent<RectTransform>().anchoredPosition = pos;
            i++;
        }
        i = 0;
        foreach(GameObject obj in cardsBackDeck) {
            Vector3 pos = new Vector3((i % 5) * sizeCard.x + offSetPos.x, (int)(i / 5) * -sizeCard.y + (offSetPos.y -sizeCard.y) + offsetDeck, 0);
            obj.GetComponent<RectTransform>().anchoredPosition = pos;
            i++;
        }
    }

    public float UpdateOffset(List<GameObject> lst, int nbRows, float offset)
    {
        float offsetUpdated = offset + -Input.mouseScrollDelta.y * speedMouseWheel;
        if (offsetUpdated < 0)
            offsetUpdated = 0;
        if (offsetUpdated > ((lst.Count / 5) - nbRows) * sizeCard.y + 40)
            offsetUpdated = ((lst.Count / 5) - nbRows) * sizeCard.y + 40;
        return offsetUpdated;
    }

    private bool isInRect(RectTransform rect)
    {
        Vector2 localMousePosition = rect.InverseTransformPoint(Input.mousePosition);
        return rect.rect.Contains(localMousePosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRect(canvasAll.GetComponent<RectTransform>()))
            offsetAll = UpdateOffset(cardsBackAll, 4, offsetAll);
        else if (isInRect(canvasDeck.GetComponent<RectTransform>()))
            offsetDeck = UpdateOffset(cardsBackDeck, 3, offsetDeck);
        SetPosWithOffset();
    }
    #endregion BackCards
}
