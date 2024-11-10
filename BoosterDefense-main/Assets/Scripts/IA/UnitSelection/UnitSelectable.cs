using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitSelectable : MonoBehaviour
{
    void Start()
    {
        UnitSelection.instance.unitlist.Add(gameObject);
    }

    void OnDestroy()
    {
        UnitSelection.instance.unitlist.Remove(gameObject);
    }
}
