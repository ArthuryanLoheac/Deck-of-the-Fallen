using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    private Button button;
    public TMP_Text text;
    public int levelId;
    public GameObject[] stars;
    public GameObject[] starsPerfect;
    public GameObject AllStars;


    void SetStars()
    {
        foreach (GameObject starObj in stars)
            starObj.SetActive(false);
        foreach (GameObject starObj in starsPerfect)
            starObj.SetActive(false);

        int star = LevelManager.instance.levels[levelId].stars;
        if (star == 4) {
            foreach (GameObject starObj in starsPerfect)
                starObj.SetActive(true);
        } else {
            if (star >= 1) 
                stars[0].SetActive(true);
            if (star >= 2) 
                stars[1].SetActive(true);
            if (star >= 3)
                stars[2].SetActive(true);
        }
    }

    void Start()
    {
        button = GetComponent<Button>();
        text.text = LevelManager.instance.levels[levelId].NameLevel;

        if (LevelManager.instance.levels[levelId].stars != -1 ) {
            AllStars.SetActive(true);
            SetStars();
        } else {
            AllStars.SetActive(false);
        }
    }

    void Update()
    {
        button.interactable = LevelManager.instance.levels[levelId].isDeblocked;
        if (button.interactable && DeckCardsManager.instance.deck.Count < DeckCardsManager.instance.nbCardMin) {
            button.interactable = false;
        }
    }

    public void LoadSceneLevel()
    {
        if (LevelManager.instance.levels[levelId].isDeblocked) {
            SceneManager.LoadScene(LevelManager.instance.levels[levelId].SceneName);
        }
    }
}
