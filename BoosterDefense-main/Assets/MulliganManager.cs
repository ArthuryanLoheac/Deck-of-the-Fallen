using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulliganManager : MonoBehaviour
{
    public bool isInMulligan;
    public static MulliganManager instance;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        isInMulligan = false;
    }

    IEnumerator MulliganDraw()
    {
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
