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
    public TMP_Text TextButtonMulligan;
    List<CardStats> cards = new List<CardStats>();
    List<GameObject> cardsObj = new List<GameObject>();
    float widthCard = 0;
    int cardToRemove = 0;

    public void Awake()
    {
        instance = this;
        buttonTake.SetActive(false);
        buttonMulligan.SetActive(false);
    }

    public void Start()
    {
        isInMulligan = false;
        widthCard = mulliganPrefabCard.GetComponent<RectTransform>().sizeDelta.x;
    }

    public void ValidMulligan()
    {
        isInMulligan = false;
        buttonTake.SetActive(false);
        buttonMulligan.SetActive(false);
        DeckManager.instance.SpawnCards(cards);
        ClearMulligan();
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
        buttonTake.GetComponent<Button>().interactable = true;
        if ((cardToRemove+1) < draw)
            buttonMulligan.GetComponent<Button>().interactable = true;
    }

    public void Mulligan()
    {
        isInMulligan = true;
        StartCoroutine(MulliganDraw());
    }
}
