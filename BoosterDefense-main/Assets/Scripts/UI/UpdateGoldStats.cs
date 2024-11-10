using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateGoldStats : MonoBehaviour
{
    public TMP_Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = RessourceManager.instance.GetRessourceAmount(RessourceType.gold).ToString();
    }
}
