using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulliganManager : MonoBehaviour
{
    public bool isInMulligan;
    public static MulliganManager instance;
    public GameObject mulliganPrefabCard;
    float widthCard = 0;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        isInMulligan = false;
        widthCard = mulliganPrefabCard.GetComponent<RectTransform>().sizeDelta.x;
    }

    IEnumerator MulliganDraw()
    {
        int draw = 5;
        for (int i = 0; i < draw; i++)
        {
            Vector3 pos = transform.position;
            pos.x += (i - (draw / 2)) * (widthCard * 1.5f);
            Instantiate(mulliganPrefabCard, pos, Quaternion.identity, transform);
        }
        yield return new WaitForSeconds(5.0f);
        isInMulligan = false;
        Debug.Log("MULLIGAN");
    }

    public void Mulligan()
    {
        isInMulligan = true;
        StartCoroutine(MulliganDraw());
    }
}
