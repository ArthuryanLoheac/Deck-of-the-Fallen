using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetParamData : MonoBehaviour
{
    public TMP_Text txt;
    void Start()
    {
        txt.text = PlayerPrefs.GetFloat("SensivityCam", 1.0f).ToString();
    }
    public void SetCamSens(Slider slider)
    {
        ParameterDatas.instance.SetSensivityCam(slider);
        txt.text = slider.value.ToString("F2");
    }
}
