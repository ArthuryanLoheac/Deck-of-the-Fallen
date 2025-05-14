using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonWinLoose : MonoBehaviour
{
    public void DefeatButton()
    {
        RessourceManager.instance.EndGame();
        SceneManager.LoadScene("Menu");
    }
    public void WinButton()
    {
        RessourceManager.instance.EndGame();
        LevelManager.instance.LevelWin(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Menu");
    }
}
