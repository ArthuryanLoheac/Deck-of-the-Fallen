using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
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
    public TMP_Text TextButtonMulligan;
    List<CardStats> cards = new List<CardStats>();
    List<int> cardsDiscarded = new List<int>();
    List<GameObject> cardsObj = new List<GameObject>();
    float widthCard = 0;
    int cardToRemove = 0;

    public void Awake()
    {
        instance = this;
        buttonTake.SetActive(false);
        buttonMulligan.SetActive(false);
        buttonDiscard.SetActive(false);
    }

    public void Start()
    {
        isInMulligan = false;
        widthCard = mulliganPrefabCard.GetComponent<RectTransform>().sizeDelta.x;
    }

    void RemoveCardDiscarded()
    {
        List<CardStats> cardsStatDiscarded = new List<CardStats>();
        foreach (int i in cardsDiscarded)
            cardsStatDiscarded.Add(cards[i]);
        foreach (CardStats stat in cardsStatDiscarded)
            cards.Remove(stat);
    }

    public void ValidMulligan(bool skipCheck = false)
    {
        buttonTake.SetActive(false);
        buttonMulligan.SetActive(false);
        buttonDiscard.SetActive(false);
        if (cardToRemove == 0 || skipCheck == true)
        {
            RemoveCardDiscarded();
            isInMulligan = false;
            DeckManager.instance.SpawnCards(cards);
            ClearMulligan();
        }
        else
        {
            cardsDiscarded.Clear();
            buttonDiscard.GetComponent<Button>().interactable =
                cardsDiscarded.Count == cardToRemove;
            buttonDiscard.SetActive(true);
            ActiveDestroy(true);
        }
    }

    void ClearMulligan()
    {
        foreach (GameObject obj in cardsObj)
        {
            Destroy(obj);
        }
        cardsObj.Clear();
        cards.Clear();
    }

    public void UnValidMulligan()
    {
        cardToRemove += 1;
        ClearMulligan();
        DeckManager.instance.Reset();
        Mulligan();
    }

    IEnumerator MulliganDraw()
    {
        TextButtonMulligan.text = "Mulligan (-" + (cardToRemove + 1).ToString() + ")";

        int draw = 5;
        cards.Clear();
        cards = DeckManager.instance.DrawXCard(draw);
        buttonTake.SetActive(true);
        buttonMulligan.SetActive(true);
        buttonTake.GetComponent<Button>().interactable = false;
        buttonMulligan.GetComponent<Button>().interactable = false;
        for (int i = 0; i < draw; i++)
        {
            Vector3 pos = transform.position;
            pos.x += (i - (draw / 2)) * (widthCard * 1.5f);
            GameObject card = Instantiate(mulliganPrefabCard, pos, Quaternion.identity, transform);
            card.GetComponent<Card>().SetStats(cards[i]);
            cardsObj.Add(card);
            yield return new WaitForSeconds(0.1f);
        }
        setIds();
        buttonTake.GetComponent<Button>().interactable = true;
        if ((cardToRemove + 1) < draw)
            buttonMulligan.GetComponent<Button>().interactable = true;
    }

    public void Mulligan()
    {
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
