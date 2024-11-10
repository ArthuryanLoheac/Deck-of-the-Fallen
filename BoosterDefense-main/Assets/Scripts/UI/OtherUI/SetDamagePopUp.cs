using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetDamagePopUp : MonoBehaviour
{
    public TMP_Text text;
    public Color positive;
    public Color fire;
    public Color negative;


    public void SetValueText(float value, TypeDamage typeDamage=TypeDamage.Normal)
    {
        if (typeDamage == TypeDamage.Heal)  {
            text.text = (-value).ToString();
            text.color = positive;
        } else if (typeDamage == TypeDamage.Fire) {
            text.text = value.ToString();
            text.color = fire;
        } else {
            text.text = value.ToString();
            text.color = negative;
        }
    }
}
