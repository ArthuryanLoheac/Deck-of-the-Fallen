using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParameterManager : MonoBehaviour
{
    static public ParameterManager instance;
    public Slider sliderMusic, sliderSound, sliderCamSens;
    public GameObject canva;
    public bool isOpen = false;
    // Start is called before the first frame update

    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
            Destroy(gameObject);
    }

    void Start()
    {
        OpenCloseParameters(false);
    }

    void setParams()
    {
        if (SoundManager.instance == null)
            return;
        sliderMusic.value = SoundManager.instance.volumeMusic;
        sliderSound.value = SoundManager.instance.volumeSound;
        sliderCamSens.value = ParameterDatas.instance.SensivityCam;
    }

    public void OpenCloseParameters(bool open)
    {
        if (canva)
            canva.SetActive(open);
        isOpen = open;
        if (open)
            setParams();
    }
}
