using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberCardDeckUI : MonoBehaviour
{
    public TMP_Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = DeckManager.instance.cards.Count.ToString();
    }
}
