using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBase : MonoBehaviour
{
    public CardStats baseCard;
    public static PlaceBase instance;
    public bool BasePlaced;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //donne la carte de la base
        BasePlaced = false;
        CardsManager.instance.AddCard(baseCard);
    }

    // Update is called once per frame, 
    void Update()
    {
        
    }
}
