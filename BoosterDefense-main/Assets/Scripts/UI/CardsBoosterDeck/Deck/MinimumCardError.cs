using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinimumCardError : MonoBehaviour
{
    public GameObject obj;
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = text.text + " ("+ DeckCardsManager.instance.nbCardMin.ToString() + ")";
    }

    // Update is called once per frame
    void Update()
    {
        if (DeckCardsManager.instance.deck.Count < DeckCardsManager.instance.nbCardMin)
            obj.SetActive(true);
        else
            obj.SetActive(false);
    }
}
