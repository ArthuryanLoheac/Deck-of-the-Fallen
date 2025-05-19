using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //parameter de la base
        MulliganManager.instance.Mulligan();
        PlaceBase.instance.BasePlaced = true;
    }
}
