using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MulliganManager : MonoBehaviour
{
    public bool isInMulligan;
    public static MulliganManager instance;
    public GameObject mulliganPrefabCard;
    public GameObject buttonTake;
    public GameObject buttonMulligan;
    public GameObject buttonDiscard;
    public GameObject BackGround;
    public GameObject Title;
    public GameObject parentForCards;
    public TMP_Text TextButtonKeep;
    public TMP_Text TextMulligan;
    List<CardStats> cards = new List<CardStats>();
    [HideInInspector] public List<int> cardsDiscarded = new List<int>();
    List<GameObject> cardsObj = new List<GameObject>();
    float widthCard = 0;
    int cardToRemove = 0;
    int draw = 5;

    void setDecoration(bool b)
    {
        BackGround.SetActive(b);
        Title.SetActive(b);
    }

    public void Awake()
    {
        instance = this;
        buttonTake.SetActive(false);
        buttonMulligan.SetActive(false);
        buttonDiscard.SetActive(false);
        TextMulligan.gameObject.SetActive(false);
        setDecoration(false);
    }

    public void Start()
    {
        isInMulligan = false;
        setText();
        widthCard = mulliganPrefabCard.GetComponent<RectTransform>().sizeDelta.x;
    }

    void RemoveCardDiscarded()
    {
        List<CardStats> cardsStatDiscarded = new List<CardStats>();
        foreach (int i in cardsDiscarded)
            cardsStatDiscarded.Add(cards[i]);
        foreach (CardStats stat in cardsStatDiscarded)
        {
            DeckManager.instance.AddCardDeckEnd(stat);
            cards.Remove(stat);
        }
    }

    public void ValidMulligan(bool skipCheck = false)
    {
        buttonTake.SetActive(false);
        buttonMulligan.SetActive(false);
        buttonDiscard.SetActive(false);

        if (cardToRemove == 0 || skipCheck == true)
        {
            EndValidation();
        }
        else
        {
            switchToDiscard();
        }
    }

    void EndValidation()
    {
        RemoveCardDiscarded();
        isInMulligan = false;
        DeckManager.instance.SpawnCards(cards);
        ClearMulligan();
        setDecoration(false);
        TextMulligan.gameObject.SetActive(false);
    }

    void switchToDiscard()
    {
        cardsDiscarded.Clear();
        buttonDiscard.GetComponent<Button>().interactable =
            cardsDiscarded.Count == cardToRemove;
        buttonDiscard.SetActive(true);
        TextMulligan.gameObject.SetActive(false);
        Title.GetComponent<TMP_Text>().text = "DISCARD " + cardToRemove.ToString() + ((cardToRemove <= 1) ? " CARD" : " CARDS");
        ActiveDestroy(true);
    }

    void ClearMulligan()
    {
        foreach (GameObject obj in cardsObj)
            Destroy(obj);
        cardsObj.Clear();
        cards.Clear();
        cardsDiscarded.Clear();
    }

    public void UnValidMulligan()
    {
        cardToRemove += 1;
        ClearMulligan();
        DeckManager.instance.Reset();
        Mulligan();
    }

    void setText()
    {
        TextButtonKeep.text = "Take " + (draw - cardToRemove).ToString();
        TextMulligan.text = "You will return " + (cardToRemove+1).ToString() + " card in your deck.";
    }

    IEnumerator MulliganDraw()
    {
        setText();
        bool NotMaxCard = ((cardToRemove + 1) < draw);
        float scale = Screen.width;

        cards.Clear();
        cards = DeckManager.instance.DrawXCard(draw);
        buttonTake.SetActive(true);
        buttonMulligan.SetActive(true);
        buttonTake.GetComponent<Button>().interactable = false;
        buttonMulligan.GetComponent<Button>().interactable = false;
        if (!NotMaxCard)
            TextMulligan.gameObject.SetActive(false);
        for (int i = 0; i < draw; i++)
        {
            Vector3 pos = transform.position;
            pos.x += (i - (draw / 2)) * (scale/9);
            GameObject card = Instantiate(mulliganPrefabCard, pos, Quaternion.identity, parentForCards.transform);
            card.GetComponent<Card>().SetStats(cards[i]);
            cardsObj.Add(card);
            yield return new WaitForSeconds(0.1f);
        }
        setIds();
        buttonTake.GetComponent<Button>().interactable = true;
        if (NotMaxCard)
            buttonMulligan.GetComponent<Button>().interactable = true;
    }

    public void Mulligan()
    {
        TextMulligan.gameObject.SetActive(true);
        setDecoration(true);
        isInMulligan = true;
        StartCoroutine(MulliganDraw());
    }

    void setIds()
    {
        int i = 0;
        foreach (GameObject c in cardsObj)
        {
            c.GetComponent<CardMulligan>().id = i;
            i++;
        }
    }
    void ActiveDestroy(bool active)
    {
        foreach (GameObject c in cardsObj) {
            c.GetComponent<CardMulligan>().ActiveDestroy(active);
        }
    }

    public void RemoveCard(int id)
    {
        bool Discard = true;

        foreach (int c in cardsDiscarded)
        {
            if (c == id)
            {
                cardsDiscarded.Remove(c);
                Discard = false;
                break;
            }
        }
        if (Discard)
            cardsDiscarded.Add(id);
        buttonDiscard.GetComponent<Button>().interactable =
            cardsDiscarded.Count == cardToRemove;
    }
}
