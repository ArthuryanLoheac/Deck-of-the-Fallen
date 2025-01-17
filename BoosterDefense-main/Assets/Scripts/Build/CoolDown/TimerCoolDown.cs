using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum IconWaveType {
    Fight,
    Base,
    Booster,
    Wait
}

public class TimerCoolDown : MonoBehaviour
{
    public static TimerCoolDown instance;
    public Image image;
    public Color DefaultColor;
    [Range(0.0f, 1.0f)]public float MediumTime;
    public Color MediumTimeColor;
    [Range(0.0f, 1.0f)]public float ShortTime;
    public Color ShortTimeColor;
    [Header("Icons")]
    public Color colorGrey;
    public Image imageIcons;
    public Sprite iconWait;
    public Sprite iconFight;
    public Sprite iconBase;
    public Sprite iconBooster;
    
    void Awake()
    {
        instance = this;
    }

    public void UpdateCoolDown(bool isActive, float time = 0f, float timeTotal = 0f)
    {
        image.enabled = isActive;
        image.fillAmount = time / timeTotal;
        if (image.fillAmount < ShortTime) {
            image.color = ShortTimeColor;
        } else if (image.fillAmount < MediumTime) {
            image.color = MediumTimeColor;
        } else {
            image.color = DefaultColor;
        }
    }

    public void setIconWait(IconWaveType type)
    {
        if (type != IconWaveType.Wait) {
            imageIcons.color = colorGrey;
            if (type == IconWaveType.Fight) {
                imageIcons.sprite = iconFight;
            } else if (type == IconWaveType.Base) {
                imageIcons.sprite = iconBase;
            } else if (type == IconWaveType.Booster) {
                imageIcons.sprite = iconBooster;
            }
        } else {
            imageIcons.sprite = iconWait;
            imageIcons.color = Color.white;
        }
    }
}
