using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterManagerUI : MonoBehaviour
{
    public static BoosterManagerUI instance;
    public Button buttonObject;

    void Awake()
    {
        instance = this;
        DeSetupCardsUIButtons();
    }

    public void SetupCardsUIButtons()
    {
        buttonObject.gameObject.SetActive(true);
    }

    public void TakeCards()
    {
        DeSetupCardsUIButtons();
        BoosterManager.instance.CloseBoosterOpening();
    }

    public void DeSetupCardsUIButtons()
    {
        buttonObject.gameObject.SetActive(false);
    } 
}
