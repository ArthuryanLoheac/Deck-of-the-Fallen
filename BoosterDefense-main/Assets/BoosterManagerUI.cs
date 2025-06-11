using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum RevealState
{
    NOT_SETUP,
    NOT_REVEAL,
    REVEALED,
}

public class BoosterManagerUI : MonoBehaviour
{
    public static BoosterManagerUI instance;
    public TMP_Text txtButton;
    public Button buttonObjectReveal;
    public RevealState isReveal = RevealState.NOT_SETUP;

    void Awake()
    {
        instance = this;
        DeSetupCardsUIButtons();
        isReveal = RevealState.NOT_SETUP;
    }

    public void SetupCardsUIButtons()
    {
        buttonObjectReveal.gameObject.SetActive(true);
        buttonObjectReveal.interactable = false;
        txtButton.text = "Reveal";
    }

    public void ActiveCardsUiButtons()
    {
        buttonObjectReveal.interactable = true;
    }

    public void TakeCards()
    {
        DeSetupCardsUIButtons();
        BoosterManager.instance.CloseBoosterOpening();
    }

    public void RevealCard()
    {
        BoosterDrawCardUI.instance.RevealAllCards();
        txtButton.text = "Take";
        isReveal = RevealState.REVEALED;
    }

    public void TakeOrReveal()
    {
        if (isReveal == RevealState.REVEALED)
        {
            TakeCards();
        } else if (isReveal == RevealState.NOT_REVEAL)
        {
            RevealCard();
        }
    }

    public void DeSetupCardsUIButtons()
    {
        buttonObjectReveal.gameObject.SetActive(false);
    }

    void Update()
    {
        if (BoosterDrawCardUI.instance.isRevealedAll() && isReveal == RevealState.NOT_REVEAL)
        {
            RevealCard();
        }
    }
}
