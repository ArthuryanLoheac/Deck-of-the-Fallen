using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonWinLoose : MonoBehaviour
{
    public void DefeatButton()
    {
        SceneManager.LoadScene("Menu");
    }
    public void WinButton()
    {
        LevelManager.instance.LevelWin(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Menu");
    }
}
