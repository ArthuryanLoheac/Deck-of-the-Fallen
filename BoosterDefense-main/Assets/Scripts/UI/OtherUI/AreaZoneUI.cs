using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaZoneUI : MonoBehaviour
{
    public float valueDefault = 0;
    public float valueWeapon = 0;
    public float valueRessource = 0;
    public float valueDetection = 0;

    public Weapon weapon;
    public NPCStats npcStats;


    void Start()
    {
        GameObject areaInteract;
        if (valueDefault != 0) {
            areaInteract = Instantiate(UIManager.instance.getDefaultArea(), transform);
            areaInteract.transform.localScale = areaInteract.transform.localScale * valueDefault;
        }
        if (valueWeapon != 0) {
            areaInteract = Instantiate(UIManager.instance.getWeaponArea(), transform);
            areaInteract.transform.localScale = areaInteract.transform.localScale * valueWeapon;
        }
        if (valueRessource != 0) {
            areaInteract = Instantiate(UIManager.instance.getCollectArea(), transform);
            areaInteract.transform.localScale = areaInteract.transform.localScale * valueRessource;
        }
        if (valueDetection != 0) {
            areaInteract = Instantiate(UIManager.instance.getDetectionArea(), transform);
            areaInteract.transform.localScale = areaInteract.transform.localScale * valueDetection;
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
