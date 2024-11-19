using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterDrawCardUI : MonoBehaviour
{
    public GameObject CardPrefab;
    public static BoosterDrawCardUI instance;
    public bool isDrawing = false;
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        CardPrefab.SetActive(false);
        isDrawing = false;
    }

    public void SetupCard(CardStats stats, int rarity)
    {
        CardPrefab.GetComponent<Card>().SetStats(stats, true);
        CardPrefab.SetActive(true);
        if (rarity == 0)
            CardPrefab.GetComponent<Animation>().Play("SpawnCardDraw");
        if (rarity == 1)
            CardPrefab.GetComponent<Animation>().Play("SpawnCardRareDraw");
    }
    public void DesactiveCard()
    {
        CardPrefab.SetActive(false);
    }
}
