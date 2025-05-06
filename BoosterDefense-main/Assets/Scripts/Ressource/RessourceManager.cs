using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RessourceType
{
    gold,
    goldInGame,
    scraps,
    food,
}

public class RessourceManager : MonoBehaviour
{
    public static RessourceManager instance;
    public LayerMask ressourceLayer;
    public LayerMask AllLayer;
    public int scraps = 0, gold = 0, food = 0, goldInGame = 0;

    public Sprite scrapsIcon, goldIcon, foodIcon;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void EndGame() {
        Debug.Log("EndGame");
        gold += goldInGame;
        ClearRessources();
        Debug.Log(gold);
    }
    public void ClearRessources() {
        Debug.Log("- ClearRessources");
        goldInGame = 0;
        scraps = 0;
        food = 0;
    }

    public int GetRessourceAmount(RessourceType nameRessource)
    {
        if (nameRessource == RessourceType.scraps) {
            return scraps;
        } else if (nameRessource == RessourceType.gold) {
            return gold;
        } else if (nameRessource == RessourceType.goldInGame) {
            return goldInGame;
        } else if (nameRessource == RessourceType.food) {
            return food;
        }
        return -1;
    }

    public Sprite GetRessourceIcon(RessourceType nameRessource)
    {
        if (nameRessource == RessourceType.scraps) {
            return scrapsIcon;
        } else if (nameRessource == RessourceType.goldInGame) {
            return goldIcon;
        } else if (nameRessource == RessourceType.gold) {
            return goldIcon;
        } else if (nameRessource == RessourceType.food) {
            return foodIcon;
        }
        return null;
    }

    public void AddRessource(RessourceType nameRessource, int amount)
    {
        if (nameRessource == RessourceType.scraps) {
            scraps += amount;
        } else if (nameRessource == RessourceType.gold) {
            gold += amount;
        } else if (nameRessource == RessourceType.food) {
            food += amount;
        } else if (nameRessource == RessourceType.goldInGame) {
            goldInGame += amount;
        }
    }
}
