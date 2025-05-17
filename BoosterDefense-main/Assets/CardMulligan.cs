using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardMulligan : MonoBehaviour
{
    public int id;
    public GameObject button;
    public TMP_Text txt_button;
    bool discard;

    public void Start()
    {
        ActiveDestroy(false);
        discard = true;
        txt_button.text = discard ? "Discard" : "Keep";
    }

    public void DestroyCard()
    {
        discard = !discard;
        txt_button.text = discard ? "Discard" : "Keep";
        MulliganManager.instance.RemoveCard(id);
    }

    public void ActiveDestroy(bool active)
    {
        button.SetActive(active);
    }
}
