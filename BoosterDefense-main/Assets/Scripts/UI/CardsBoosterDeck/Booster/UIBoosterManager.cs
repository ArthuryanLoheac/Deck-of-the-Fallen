using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBoosterManager : MonoBehaviour
{
    public static UIBoosterManager instance;
    public GameObject prefabBooster;
    public Transform Canvas;
    private float offset;
    public float speedMouseWheel;

    public int SizeCard = 220;
    public int CardSeparation = 420;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        offset = 0;
        RefreshShop();
    }
    public void RefreshShop()
    {
        int i = 0;
        foreach (BoosterStats booster in BoosterManager.instance.boosterOwned) {
            Vector3 pos = new Vector3((CardSeparation * i) + SizeCard, 0, 0);
            GameObject BoosterCreated = Instantiate(prefabBooster, Canvas);
            BoosterCreated.GetComponent<RectTransform>().anchoredPosition = pos;
            BoosterCreated.GetComponent<Booster>().SetStats(booster);
            i++;
        }
    }

    void Update()
    {
        offset += Input.mouseScrollDelta.y * speedMouseWheel;
        offset = Mathf.Max(-((Canvas.childCount - 5) * CardSeparation + SizeCard + 60), offset);
        offset = Mathf.Min(0, offset);
        for (int i = 0; i < Canvas.childCount; i++  ) {
            Canvas.GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector3((CardSeparation * i) + SizeCard + offset, 0, 0);
        }   
    }
}
