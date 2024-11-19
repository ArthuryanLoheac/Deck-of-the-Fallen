using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetNbCardDeckUI : MonoBehaviour
{
    public Color Less;
    public Color More;
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = DeckCardsManager.instance.deck.Count.ToString();
        if (DeckCardsManager.instance.deck.Count < DeckCardsManager.instance.nbCardMin) {
            text.color = Less;
        } else {
            text.color = More;
        }
    }
}
