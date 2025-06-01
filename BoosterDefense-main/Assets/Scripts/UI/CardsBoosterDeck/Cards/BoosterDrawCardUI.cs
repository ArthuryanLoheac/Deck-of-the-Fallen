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
        bool isFlipped = true;
        if (rarity == 0)
            isFlipped = false;
        obj.GetComponent<Card>().SetStats(stats, isFlipped);
        obj.SetActive(true);
    }

    public IEnumerator SetupCards(List<CardStats> stats, List<int> rarity)
    {
        float scale = Screen.width;
        DesactiveCard();

        for (int i = 0; i < stats.Count; i++)
        {
            Vector3 pos = transform.GetComponent<RectTransform>().position;
            float ipos = i - (stats.Count / 2.0f) + 0.5f;
            pos.x += ipos * (scale / 8.0f);
            CardInstantiate.Add(Instantiate(CardPrefab, pos, Quaternion.identity, transform));
            GameObject cardCreated = CardInstantiate[i];
            cardCreated.transform.GetComponent<RectTransform>().localScale = new Vector3(3, 3, 3);
            SetupCard(stats[i], rarity[i], cardCreated);
            cardCreated.GetComponent<Animation>().Play("CardFlipSpawn");
            yield return new WaitForSeconds(0.2f);
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
