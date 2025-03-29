using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSoundSlider : MonoBehaviour
{
    public void SetVolume()
    {
        SoundManager.instance.SetVolume(GetComponent<UnityEngine.UI.Slider>());
    }
}
