using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterDrawCardUI : MonoBehaviour
{
    public GameObject CardPrefab;
    public static BoosterDrawCardUI instance;
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        CardPrefab.SetActive(false);
    }

    public void SetupCard(CardStats stats)
    {
        CardPrefab.GetComponent<Card>().SetStats(stats, true);
        CardPrefab.SetActive(true);
    }
    public void DesactiveCard()
    {
        CardPrefab.SetActive(false);
    }
}
