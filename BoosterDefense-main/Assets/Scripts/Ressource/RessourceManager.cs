using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RessourceType
{
    gold,
    scraps,
    food,
}

public class RessourceManager : MonoBehaviour
{
    public static RessourceManager instance;
    public LayerMask ressourceLayer;
    public LayerMask AllLayer;
    public int scraps = 0, gold = 1, food = 2;

    public Sprite scrapsIcon, goldIcon, foodIcon;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    public int GetRessourceAmount(RessourceType nameRessource)
    {
        if (nameRessource == RessourceType.scraps) {
            return scraps;
        } else if (nameRessource == RessourceType.gold) {
            return gold;
        } else if (nameRessource == RessourceType.food) {
            return food;
        }
        return -1;
    }

    public Sprite GetRessourceIcon(RessourceType nameRessource)
    {
        if (nameRessource == RessourceType.scraps) {
            return scrapsIcon;
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
        }
    }
}
