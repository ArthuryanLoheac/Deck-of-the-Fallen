using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyBooster : MonoBehaviour
{
    public Booster booster;
    public void Buy(bool AddCardToHand = false)
    {
        RessourceManager.instance.AddRessource(RessourceType.gold, -booster.price);
        BoosterManager.instance.OpenBooster(booster.boosterStats, AddCardToHand);
    }
}
