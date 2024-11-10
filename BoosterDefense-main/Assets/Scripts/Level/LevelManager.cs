using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class level {
    public string SceneName;
    public string NameLevel;
    public bool isDeblocked;
    public string SceneNameDeblock;
    //public int nbStars ...
}

public class LevelManager : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now

    public static LevelManager instance;
    public level[] levels;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public level GetLevel(string levelSceneName)
    {
        foreach (level lev in levels) {
            if (lev.SceneName == levelSceneName) {
                return lev;
            }
        }
        return null;
    }

    public void LevelWin(string levelSceneName)
    {
        foreach (level lev in levels) {
            if (lev.SceneName == levelSceneName && lev.isDeblocked) {
                if (lev.SceneNameDeblock != "")
                    GetLevel(lev.SceneNameDeblock).isDeblocked = true;
            }
        }
    }
}
