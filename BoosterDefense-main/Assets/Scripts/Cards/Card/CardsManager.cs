using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour
{
    public GameObject prefabCard;
    public int numberCards = 0;
    public int numberCardsShow = 6;
    public static CardsManager instance;
    public int cursorCards = 0;
    public Button LeftCursor;
    public Button RightCursor;
    public GameObject prefabCardSlot;
    [HideInInspector]public GameObject CardGrabbed;
    public GameObject SellCard;
    private float separationX = 70f;
    private float separationY = 10f;
    private float cardPosYDown = 30f;
    private float posDiffX;

    void Awake()
    {
        instance = this;
    }

    public void UpdateCursor()
    {
        //check cursor superior to max and min values
        if (cursorCards > numberCards - numberCardsShow)
            cursorCards = numberCards - numberCardsShow;
        if (cursorCards < 0)
            cursorCards = 0;

        //Active or desactive buttons if at max or min values
        if (cursorCards > 0)
            LeftCursor.gameObject.SetActive(true);
        else
            LeftCursor.gameObject.SetActive(false);
        if (cursorCards < numberCards - numberCardsShow)
            RightCursor.gameObject.SetActive(true);
        else
            RightCursor.gameObject.SetActive(false);
    }

    private int GetNumberCardToShow()
    {
        if (numberCards > numberCardsShow)
            return numberCardsShow;
        else
            return transform.childCount;
    }

    private float getValueRotation(int count, int nbCardToShow)
    {
        float valueRotation = count - nbCardToShow / 2;
        valueRotation = (valueRotation >= 0 && nbCardToShow%2==0)? valueRotation + 1: valueRotation;
        return valueRotation;
    }

    public void UpdatePosCards()
    {
        int count = 0;

        UpdateCursor();
        int nbCardToShow = GetNumberCardToShow();
        UpdateCursor();

        //active or desactive cards in the hands
        for (int i = 0; i < cursorCards; i++)
            transform.GetChild(i).gameObject.SetActive(false);
        for (int i = cursorCards; i < Mathf.Min(transform.childCount, cursorCards + nbCardToShow); i++) {
            float valueRotation = getValueRotation(count, nbCardToShow);

            if (transform.GetChild(i).GetComponent<DraggableCard>().Zoomed){
                transform.GetChild(i).localScale = new Vector3(1.3f,1.3f,1.3f);
                transform.GetChild(i).transform.eulerAngles = Vector3.zero;

                var rectTransform = transform.GetChild(i).GetComponent<RectTransform>();
                float height = rectTransform.rect.height;
                transform.GetChild(i).transform.position = new Vector3(
                    transform.position.x + (posDiffX * count) - ((nbCardToShow - 1) * (posDiffX/2) + (valueRotation * 10f)),
                    (height/2) * (Screen.height / 1080f), transform.position.z);
            } else { 
                transform.GetChild(i).localScale = new Vector3(1f,1f,1f);
                Vector3 vec = transform.GetChild(i).transform.eulerAngles;
                vec.z = valueRotation * -5f;
                transform.GetChild(i).transform.eulerAngles = vec;
                transform.GetChild(i).transform.position = new Vector3(
                    transform.position.x + (posDiffX * count) - ((nbCardToShow - 1) * (posDiffX/2) + (valueRotation * 10f)),
                    transform.position.y - (Mathf.Abs(valueRotation) * GetPosDiffY()) - cardPosYDown, transform.position.z);
            }
            
            transform.GetChild(i).gameObject.SetActive(true);
            count++;
        }
        for (int i = Mathf.Min(transform.childCount,cursorCards + nbCardToShow); i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }

    public void UpdatePosCardsGrabbed()
    {
        int count = 0;

        UpdateCursor();
        int nbCardToShow = Mathf.Min(GetNumberCardToShow(), numberCardsShow - 1);
        UpdateCursor();

        int idPosGrrabbed = CardGrabbed.GetComponent<DraggableCard>().GetCardGarbPosition();

        //active or desactive cards in the hands
        for (int i = 0; i < cursorCards; i++)
            transform.GetChild(i).gameObject.SetActive(false);
        for (int i = cursorCards; i < cursorCards + nbCardToShow; i++) {
            if (idPosGrrabbed == i)
                count++;
            float valueRotation = getValueRotation(count, nbCardToShow + 1);
            Vector3 vec = transform.GetChild(i).transform.eulerAngles;
            vec.z = valueRotation * -5f;
            transform.GetChild(i).transform.eulerAngles = vec;
            transform.GetChild(i).gameObject.SetActive(true);
            transform.GetChild(i).transform.position = new Vector3(
                transform.position.x + (posDiffX * count) - (nbCardToShow * (posDiffX/2) + (valueRotation * 10f)),
                transform.position.y - (Mathf.Abs(valueRotation) * GetPosDiffY()) - cardPosYDown, transform.position.z);
            count++;
        }
        for (int i = cursorCards + nbCardToShow; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }

    public Card GetCardInHand(string nameCard)
    {
        for(int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).GetComponent<Card>().cardStats.name == nameCard) {
                return transform.GetChild(i).gameObject.GetComponent<Card>();
            }
        }
        return null;
    }

    void Update()
    {
        UpdatePosDiffX();
        if (CardGrabbed) {
            UpdatePosCardsGrabbed();
            if (CardGrabbed.GetComponent<Card>().cardStats.CanBeSold) {
                SellCard.SetActive(true);
                SellCard.GetComponent<OnCardDropOnSell>().SetValues();
            }
        } else {
            UpdatePosCards();
            SellCard.SetActive(false);
        }
    }

    public void AddCard(CardStats cardToAdd)
    {
        if (GetCardInHand(cardToAdd.name) != null){
            GetCardInHand(cardToAdd.name).cardCount += 1;
        } else {
            GameObject card = Instantiate(prefabCard, transform);
            card.GetComponent<SpawnGhost>().ghostToSpawn = cardToAdd.ghostToSpawn;
            card.GetComponent<Card>().SetStats(cardToAdd);
            numberCards += 1;
        }
        UpdatePosCards();
    }

    public void AddCardToCardUsed(CardStats obj)
    {
        DeckManager.instance.AddCardUsed(obj);
    }

    public void RemoveCard(GameObject obj)
    {
        obj.GetComponent<Card>().cardCount -= 1;
        UpdatePosCards();
    }

    public void MoveCursor(int amount)
    {
        cursorCards += amount;
        UpdatePosCards();
    }

    public void RemovePackCard(GameObject obj)
    {
        numberCards -= 1;
        Destroy(obj);
        UpdatePosCards();
    }

    void UpdatePosDiffX()
    {
        posDiffX = separationX * (Screen.width / 1280f);
    }
    float GetPosDiffY()
    {
        return separationY * (Screen.height / 1080f);
    }

    void Start()
    {
        UpdatePosDiffX();
        UpdatePosCards();
    }
}
