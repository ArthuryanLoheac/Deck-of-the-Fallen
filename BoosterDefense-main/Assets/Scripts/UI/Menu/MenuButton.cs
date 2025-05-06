using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void GoToScene(string Scene)
    {
        SceneManager.LoadScene(Scene);
    }
    public void GoToSceneAndClearGame(string Scene)
    {
        RessourceManager.instance.ClearRessources();
        SceneManager.LoadScene(Scene);
    }
    public void GoToSceneAndDestroyLevelManager(string Scene)
    {
        Destroy(LevelManager.instance.gameObject);
        SceneManager.LoadScene(Scene);
    }
}
