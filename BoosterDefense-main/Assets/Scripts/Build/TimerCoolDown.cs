using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCoolDown : MonoBehaviour
{
    public static TimerCoolDown instance;
    public Image image;
    public Image imageBG;
    public Color DefaultColor;
    [Range(0.0f, 1.0f)]public float MediumTime;
    public Color MediumTimeColor;
    [Range(0.0f, 1.0f)]public float ShortTime;
    public Color ShortTimeColor;
    
    
    void Awake()
    {
        instance = this;
    }

    public void UpdateCoolDown(bool isActive, float time = 0f, float timeTotal = 0f)
    {
        image.enabled = isActive;
        imageBG.enabled = isActive;
        image.fillAmount = time / timeTotal;
        if (image.fillAmount < ShortTime) {
            image.color = ShortTimeColor;
        } else if (image.fillAmount < MediumTime) {
            image.color = MediumTimeColor;
        } else {
            image.color = DefaultColor;
        }
    }
}
