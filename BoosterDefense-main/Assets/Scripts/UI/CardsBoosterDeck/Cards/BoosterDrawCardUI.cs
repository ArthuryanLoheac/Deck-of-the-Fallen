using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterDrawCardUI : MonoBehaviour
{
    public GameObject CardPrefab;
    private List<GameObject> CardInstantiate;
    public static BoosterDrawCardUI instance;
    public bool isDrawing = false;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        CardInstantiate = new List<GameObject>();
        CardPrefab.SetActive(false);
        isDrawing = false;
    }
    void SetupCard(CardStats stats, int rarity, GameObject obj)
    {
        obj.GetComponent<Card>().SetStats(stats);
        obj.SetActive(true);
        //if (rarity == 0)
        //    obj.GetComponent<Animation>().Play("SpawnCardDraw");
        //if (rarity == 1)
        //    obj.GetComponent<Animation>().Play("SpawnCardRareDraw");
    }

    public void SetupCards(List<CardStats> stats, List<int> rarity)
    {
        float scale = Screen.width;
        DesactiveCard();
        for (int i = 0; i < stats.Count; i++)
        {
            Vector3 pos = transform.GetComponent<RectTransform>().position;
            float ipos = i - (stats.Count / 2.0f) + 0.5f;
            Debug.Log(ipos);
            pos.x += ipos * (scale/10.0f);
            CardInstantiate.Add(Instantiate(CardPrefab, pos, Quaternion.identity, transform));
            CardInstantiate[i].transform.GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);
            SetupCard(stats[i], rarity[i], CardInstantiate[i]);
        }
    }
    public void DesactiveCard()
    {
        foreach (GameObject obj in CardInstantiate)
        {
            Destroy(obj);
        }
        CardInstantiate.Clear();
        CardPrefab.SetActive(false);
    }
}
