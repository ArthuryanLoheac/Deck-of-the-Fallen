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
    public TypeCardArt artType = TypeCardArt.NORMAL_ART;
    [Header("HP")]
    public bool hasHp;
    public Rarity rarity;
    [Header("Offset")]
    [Range(-38f, 38f)]
    public float offsetTop = 0.0f; // -38 à 38
    
    public static bool operator ==(CardStats a, CardStats b)
    {
        if (ReferenceEquals(a, b))
            return true;
        if (a is null || b is null)
            return false;
        return a.name == b.name && a.artType == b.artType;
    }

    public static bool operator !=(CardStats a, CardStats b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
            return true;
        if (obj == null || GetType() != obj.GetType())
            return false;
        CardStats other = (CardStats)obj;
        return name == other.name && artType == other.artType;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + (name != null ? name.GetHashCode() : 0);
        hash = hash * 23 + artType.GetHashCode();
        return hash;
    }
}
