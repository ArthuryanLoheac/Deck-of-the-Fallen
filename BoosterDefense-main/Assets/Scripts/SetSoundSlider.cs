using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSoundSlider : MonoBehaviour
{
    public void SetVolumeMusic(Slider slider)
    {
        SoundManager.instance.SetVolumeMusic(slider);
    }
    public void SetVolumeSound(Slider slider)
    {
        SoundManager.instance.SetVolumeSound(slider);
    }
}
