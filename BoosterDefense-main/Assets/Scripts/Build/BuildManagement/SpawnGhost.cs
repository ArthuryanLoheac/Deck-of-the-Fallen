using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGhost : MonoBehaviour
{
    [HideInInspector]public GameObject ghostToSpawn;
    private Card card;


    void Start()
    {
        card = GetComponent<Card>();
    }
    public void spawnTheGhost()
    {
        //sans position
        if (!BuildManager.instance.isBuilding && card.cardStats != null){
            BuildManager.instance.isBuilding = true;
            GameObject ghost = Instantiate(ghostToSpawn);
            ghost.GetComponent<placementInGrid>().SetValues(card.cardStats, transform.GetSiblingIndex());
            CardsManager.instance.RemoveCard(gameObject);
        }
    }
    public void spawnTheGhost(Vector3 pos)
    {
        //Avec position
        if (!BuildManager.instance.isBuilding && card.cardStats != null){
            BuildManager.instance.isBuilding = true;
            GameObject ghost = Instantiate(ghostToSpawn, pos, Quaternion.identity);
            ghost.GetComponent<placementInGrid>().SetValues(card.cardStats, transform.GetSiblingIndex());
            CardsManager.instance.RemoveCard(gameObject);
        }
    }
}
