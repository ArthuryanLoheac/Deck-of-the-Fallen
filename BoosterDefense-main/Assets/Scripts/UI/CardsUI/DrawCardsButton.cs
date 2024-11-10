using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DrawCardsButton : MonoBehaviour
{
    public static DrawCardsButton instance;
    public int drawTime;
    public int drawTimeMax;
    public int bonusDraw;
    private Button button;
    public TMP_Text textDrawRemaining;

    public void ResetDrawTime()
    {
        drawTime = drawTimeMax;
        for (int i = 0; i < drawTime; i++) {
            DeckManager.instance.DrawAndSpawnCardValue();
        }
        drawTime = 0;
    }

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        button = GetComponent<Button>();
        Active(bonusDraw > 0);
    }

    public void Draw()
    {
        if (bonusDraw > 0) {
            DeckManager.instance.DrawAndSpawnCardValue();
            bonusDraw -= 1;
        }
    }

    public void Active(bool b)
    {
        Color col = GetComponent<Image>().color;
        if (b)
            GetComponent<Image>().color = new Color(col.r, col.g, col.b, 1);
        else
            GetComponent<Image>().color = new Color(col.r, col.g, col.b, 0);
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(b);
        }
    }

    void UpdateTextRemainingDraw()
    {
        if (drawTime <= 1)
            textDrawRemaining.text = bonusDraw.ToString() + " bonus draw";
        else
            textDrawRemaining.text = bonusDraw.ToString() + " bonus draws";
    }

    void Update()
    {
        if (!PlaceBase.instance.BasePlaced) {
            button.interactable = false;
            textDrawRemaining.text = bonusDraw.ToString() + " bonus draws";
        } else {
            if (bonusDraw <= 0) {
                Active(false);
                button.interactable = false;
            } else if (bonusDraw > 0) {
                Active(true);
                if (DeckManager.instance.cards.Count <= 0) {
                    button.interactable = false;
                } else {
                    button.interactable = true;
                }
            }
            UpdateTextRemainingDraw();
        }
    }
}
