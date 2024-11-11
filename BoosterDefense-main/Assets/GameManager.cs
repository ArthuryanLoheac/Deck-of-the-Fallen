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
    // Start is called before the first frame update
    public void Win()
    {
        WinUI.SetActive(true);
        GameEnded = true;
        TimeManager.instance.setNoSpeed();
    }
    public void Defeat()
    {
        LooseUI.SetActive(true);
        GameEnded = true;
        TimeManager.instance.setNoSpeed();
    }
}
