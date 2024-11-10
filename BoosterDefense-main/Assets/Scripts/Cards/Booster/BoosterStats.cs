using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Booster", menuName = "Booster")]
public class BoosterStats : ScriptableObject
{
    public string nameBooster;
    public int priceGold;
    public Sprite icon;

    public int nbCard;
    public int nbRare;
    public float percentSuperRare;

    public List<CardStats> listCardCommon;
    public List<CardStats> listCardRare;
    public List<CardStats> listCardSuperRare;
}
