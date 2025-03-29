using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPause;
    public static PauseMenuManager instance;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        pauseMenu.SetActive(false);
        isPause = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && ParameterManager.instance.isOpen == false) {
            if (isPause)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;
    }
}
