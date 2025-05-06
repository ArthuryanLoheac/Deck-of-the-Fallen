using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR;

public class DeckCardsManager : MonoBehaviour
{
    public static DeckCardsManager instance;
    public List<CardStats> deck;
    public List<CardStats> AllCards;
    public List<CardStats> StartHand;
    public int nbCardMin;

    void Awake()
    {
        Debug.Log(instance == null);
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void SortCardList(List<CardStats> lst)
    {
        IEnumerable<CardStats> cards = lst.OrderBy(card => card.name);
        int i = 0;
        foreach(CardStats card in cards){
            lst[i] = card;
            i++;
        }
    }

    public void SortAll()
    {
        SortCardList(AllCards);
        SortCardList(deck);
    }

    void Start()
    {
        SortCardList(deck);
        SortCardList(AllCards);
    }
}
