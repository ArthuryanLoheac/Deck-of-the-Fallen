using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParameterDatas : MonoBehaviour
{
    static public ParameterDatas instance;
    public float SensivityCam = 1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SensivityCam = PlayerPrefs.GetFloat("SensivityCam", 1.0f);
    }

    public void SetSensivityCam(Slider slider)
    {
        SensivityCam = slider.value;
        PlayerPrefs.SetFloat("SensivityCam", SensivityCam);
        PlayerPrefs.Save();
    }
}
