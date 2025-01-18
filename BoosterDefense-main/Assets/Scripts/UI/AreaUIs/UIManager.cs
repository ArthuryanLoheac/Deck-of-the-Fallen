using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject defaultArea;
    public GameObject collectArea;
    public GameObject detectionArea;
    public GameObject weaponArea;

    public GameObject statsBar;

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
