using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject LooseUI;
    public GameObject WinUI;
    public bool GameEnded = false;

    void Awake()
    {
       instance = this;
    }
    void Start()
    {
        LooseUI.SetActive(false);
        WinUI.SetActive(false);
    }

    int GetStars() 
    {
        float percent = BaseLifeManager.instance.getPercentHealth();
        int i = 0;
        if (percent < 1f) {
            if (percent == 0f)
                i = 0;
            if (percent > 0f)
                i = 1;
            if (percent > .5f)
                i = 2;
            if (percent > .75f)
                i = 3;
        } else 
            i = 4;
        
        return i;
    }

    // Start is called before the first frame update
    public void Win()
    {
        ZoomCardManager.instance.DesactiveCardZoom();
        WinUI.SetActive(true);
        GameEnded = true;
        TimeManager.instance.setNoSpeed();
        int stars = GetStars();
        WinUI.GetComponent<SetUILoose>().SetupUI(stars);
        LevelManager.instance.SetStars(stars);
    }
    public void Defeat()
    {
        ZoomCardManager.instance.DesactiveCardZoom();
        LooseUI.SetActive(true);
        GameEnded = true;
        TimeManager.instance.setNoSpeed();
        WinUI.GetComponent<SetUILoose>().SetupUI(0);
        LevelManager.instance.SetStars(0);
    }
}
