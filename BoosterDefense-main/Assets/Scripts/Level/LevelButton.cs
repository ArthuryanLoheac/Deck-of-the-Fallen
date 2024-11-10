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

    void Start()
    {
        button = GetComponent<Button>();
        text.text = LevelManager.instance.levels[levelId].NameLevel;
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
