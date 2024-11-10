using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateRessourceAmount : MonoBehaviour
{
    public RessourceType typeRessource;
    public TMP_Text text;

    void Update()
    {
        text.text = RessourceManager.instance.GetRessourceAmount(typeRessource).ToString();
    }
}
