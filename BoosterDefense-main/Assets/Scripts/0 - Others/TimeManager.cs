using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public float lowSpeed;
    public float highSpeed;
    public Button highSpeedButton;
    public Button lowSpeedButton;
    public Button normalSpeedButton;
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        setNormalSpeed(); 
    }

    private void SetActiveButton(Button button, bool b)
    {
        if (b) {
            button.interactable = true;
        } else {
            button.interactable = false;
        }
    }
    
    void Update()
    {
        if (Time.timeScale == 1) {
            SetActiveButton(normalSpeedButton, false);
            SetActiveButton(lowSpeedButton, true);
            SetActiveButton(highSpeedButton, true);
        } else if (Time.timeScale == lowSpeed) {
            SetActiveButton(normalSpeedButton, true);
            SetActiveButton(lowSpeedButton, false);
            SetActiveButton(highSpeedButton, true);
        } else if (Time.timeScale == highSpeed) {
            SetActiveButton(normalSpeedButton, true);
            SetActiveButton(lowSpeedButton, true);
            SetActiveButton(highSpeedButton, false);
        }
    }
    public void setNormalSpeed()
    {
        Time.timeScale = 1;
    }

    public void setLowSpeed()
    {
        Time.timeScale = lowSpeed;
    }
    public void setHighSpeed()
    {
        Time.timeScale = highSpeed;
    }
}
