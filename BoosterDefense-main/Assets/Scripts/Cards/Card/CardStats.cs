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

public enum Rarity
{
    Common,
    Rare,
    SuperRare
}

public enum TypeCardArt
{
    NORMAL_ART,
    FULL_ART
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
    public float sizeFont = 7f;
    [Header("Type")]
    public TypeCard type;
    [HideInInspector] public TypeCardArt artType = TypeCardArt.NORMAL_ART;
    [Header("HP")]
    public bool hasHp;
    public Rarity rarity;
    [Header("Offset")]
    [Range(-38f, 38f)]
    public float offsetTop = 0.0f; // -38 à 38

    
    public static bool operator ==(CardStats a, CardStats b)
    {
        return a.name == b.name && a.artType == b.artType;
    }

    public static bool operator !=(CardStats a, CardStats b)
    {
        return !(a == b);
    }
}
