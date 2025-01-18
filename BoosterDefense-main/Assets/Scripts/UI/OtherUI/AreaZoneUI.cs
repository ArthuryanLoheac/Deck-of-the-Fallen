using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaZoneUI : MonoBehaviour
{
    public float valueDefault = 0;

    public Weapon weapon;
    public NPCStats npcStats;


    void Start()
    {
        GameObject areaInteract;
        if (valueDefault != 0)
        {
            areaInteract = Instantiate(UIManager.instance.getDefaultArea(), transform);
            areaInteract.transform.localScale = areaInteract.transform.localScale * valueDefault;
        }
        if (weapon != null)
        {
            areaInteract = Instantiate(UIManager.instance.getWeaponArea(), transform);
            areaInteract.transform.localScale = areaInteract.transform.localScale * weapon.range;
        }
        if (npcStats != null)
        {
            areaInteract = Instantiate(UIManager.instance.getDetectionArea(), transform);
            areaInteract.transform.localScale = areaInteract.transform.localScale * npcStats.rangecollectDetection;
            areaInteract = Instantiate(UIManager.instance.getCollectArea(), transform);
            areaInteract.transform.localScale = areaInteract.transform.localScale * npcStats.rangecollect;
        }
    }
}
