using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TypeCard {
    Npc,
    Sort,
    Batiment,
    Vehicule,
    Equipement
}

public enum Rarity {
    Common,
    Rare,
    SuperRare
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardStats : ScriptableObject
{
    public new string name;
    public Sprite image;
    [Header("Price")]
    public int price;
    public RessourceType priceRessource;
    [Header("Ghost")]
    public GameObject ghostToSpawn;
    [Header("Sell")]
    public bool CanBeSold;
    public int ValueSold;
    public RessourceType SoldType;
    [Header("Deck Options")]
    public bool addToCardUsed = true;
    [Header("Description")]
    public string description;
    public string story;
    [Header("Type")]
    public TypeCard type;
    [Header("HP")]
    public bool hasHp;
    public Rarity rarity;
}
