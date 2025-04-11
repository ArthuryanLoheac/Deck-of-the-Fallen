using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckButtonMenu : MonoBehaviour
{
    public bool isButtonHandStartMenu;
    // Start is called before the first frame update
    void Start()
    {
        UpdateInteractable();
    }

    void UpdateInteractable()
    {
        if (isButtonHandStartMenu) {
            GetComponent<Button>().interactable = !DeckMenuManager.instance.isHandStartMenu;
        } else {
            GetComponent<Button>().interactable = DeckMenuManager.instance.isHandStartMenu;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInteractable();
    }

    public void switchStatus()
    {
        SoundManager.instance.PlaySound("ButtonClick");
        DeckMenuManager.instance.isHandStartMenu = isButtonHandStartMenu;
        DeckMenuManager.instance.RefreshAll();
        GetComponent<Button>().interactable = false;
    }
}
