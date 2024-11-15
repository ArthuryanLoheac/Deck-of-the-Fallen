using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaUIManager : MonoBehaviour
{
    public static AreaUIManager instance;

    public GameObject defaultArea;
    public GameObject collectArea;
    public GameObject detectionArea;
    public GameObject weaponArea;

    void Awake()
    {
        instance = this;
    }

    public GameObject getWeaponArea()
    {
        return weaponArea;
    }
    public GameObject getCollectArea()
    {
        return collectArea;
    }
    public GameObject getDetectionArea()
    {
        return detectionArea;
    }
    public GameObject getDefaultArea()
    {
        return defaultArea;
    }
}
