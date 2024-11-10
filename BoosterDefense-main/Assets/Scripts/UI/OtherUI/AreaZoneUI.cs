using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaZoneUI : MonoBehaviour
{
    public GameObject prefabAreaZone;
    private GameObject areaInteract;
    private GameObject areaDetection;
    private GameObject ObjToSpawn;

    private void WeaponArea()
    {
        areaInteract = Instantiate(prefabAreaZone, transform);
        areaInteract.GetComponent<SetSizeAreaUi>().SetSize(ObjToSpawn.GetComponent<Arbalete>().weapon.range);
    }

    private void NPCRessource()
    {
        areaInteract = Instantiate(prefabAreaZone, transform);
        areaInteract.GetComponent<SetSizeAreaUi>().SetSize(ObjToSpawn.GetComponent<IACollectRessources>().stats.rangecollect * 2);
        areaDetection = Instantiate(prefabAreaZone, transform);
        areaDetection.GetComponent<SetSizeAreaUi>().SetSize(ObjToSpawn.GetComponent<IACollectRessources>().stats.rangecollectDetection * 2);
        Vector3 ar = areaDetection.transform.position;
        ar.y -= 0.05f;
        areaDetection.transform.position = ar;
    }
    private void NPCAttack()
    {
        areaInteract = Instantiate(prefabAreaZone, transform);
        areaInteract.GetComponent<SetSizeAreaUi>().SetSize(ObjToSpawn.GetComponent<IAAttackMonster>().stats.rangecollect * 2);
        areaDetection = Instantiate(prefabAreaZone, transform);
        areaDetection.GetComponent<SetSizeAreaUi>().SetSize(ObjToSpawn.GetComponent<IAAttackMonster>().stats.rangecollectDetection * 2);
        Vector3 ar = areaDetection.transform.position;
        ar.y -= 0.05f;
        areaDetection.transform.position = ar;
    }

    void Start()
    {
        ObjToSpawn = GetComponent<placementInGrid>().objToSpawn;

        if (ObjToSpawn.tag == "Weapons") {
            WeaponArea();
        } else if (ObjToSpawn.GetComponent<IACollectRessources>()) {
            NPCRessource();
        } else if (ObjToSpawn.GetComponent<IAAttackMonster>()) {
            NPCAttack();
        }
    }
}
