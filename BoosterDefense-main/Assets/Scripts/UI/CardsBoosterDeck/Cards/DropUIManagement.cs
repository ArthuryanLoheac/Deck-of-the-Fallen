using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropUIManagement : MonoBehaviour
{
    public static DropUIManagement instance;
    public GameObject DropAll;
    public GameObject DropDeck;

    void Awake()
    {
        if (!instance) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void DesactiveDrop()
    {
        DropAll.SetActive(false);
        DropDeck.SetActive(false);
    }

    public void ActiveDrop(DeckEmplacement emplacement)
    {
        if (emplacement == DeckEmplacement.All) {
            DropAll.SetActive(false);
            DropDeck.SetActive(true);
        } else if (emplacement == DeckEmplacement.Deck) {
            DropAll.SetActive(true);
            DropDeck.SetActive(false);
        }
    }
}
