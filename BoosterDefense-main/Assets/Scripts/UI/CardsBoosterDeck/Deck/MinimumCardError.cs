using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinimumCardError : MonoBehaviour
{
    TMP_Text text;
    public string txt;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        text.text = txt + " ("+ DeckCardsManager.instance.nbCardMin.ToString() + ")";
    }

    // Update is called once per frame
    void Update()
    {
        if (DeckCardsManager.instance.deck.Count < DeckCardsManager.instance.nbCardMin)
            text.enabled = true;
        else
            text.enabled = false;
    }
}
