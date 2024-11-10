using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetValue : MonoBehaviour
{
    public TMP_Text text;
    public Image img;


    public void SetValueText(int value, RessourceType typeRessource)
    {
        text.text = "+" + value.ToString();
        img.sprite = RessourceManager.instance.GetRessourceIcon(typeRessource);
    }
}
