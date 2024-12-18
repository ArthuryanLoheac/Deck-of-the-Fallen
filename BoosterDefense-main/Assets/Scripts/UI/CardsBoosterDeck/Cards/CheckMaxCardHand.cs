using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckMaxCardHand : MonoBehaviour
{
    public string textNotActive;
    public Color colorNotActive;
    private Color colorActive;
    private string textActive;

    void Start()
    {
        textActive = transform.GetChild(0).GetComponent<TMP_Text>().text;
        colorActive = transform.GetChild(0).GetComponent<TMP_Text>().color;
    }
    // Update is called once per frame
    void Update()
    {
        if (DeckCardsManager.instance.StartHand.Count < 5) {
            GetComponent<Button>().interactable = true;
            transform.GetChild(0).GetComponent<TMP_Text>().text = textActive;
            transform.GetChild(0).GetComponent<TMP_Text>().color = colorActive;
        } else {
            GetComponent<Button>().interactable = false;
            transform.GetChild(0).GetComponent<TMP_Text>().text = textNotActive;
            transform.GetChild(0).GetComponent<TMP_Text>().color = colorNotActive;
        }
    }
}
