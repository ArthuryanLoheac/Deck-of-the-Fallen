using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckMaxCardHand : MonoBehaviour
{
    public string textNotActive;
    private string textActive;

    void Start()
    {
        textActive = transform.GetChild(0).GetComponent<TMP_Text>().text;
    }
    // Update is called once per frame
    void Update()
    {
        if (DeckCardsManager.instance.StartHand.Count < 5) {
            GetComponent<Button>().interactable = true;
            transform.GetChild(0).GetComponent<TMP_Text>().text = textActive;
        } else {
            GetComponent<Button>().interactable = false;
            transform.GetChild(0).GetComponent<TMP_Text>().text = textNotActive;
        }
    }
}
