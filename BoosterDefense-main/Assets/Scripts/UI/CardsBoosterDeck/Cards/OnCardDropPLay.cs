using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnCardDropPLay : MonoBehaviour, IDropHandler
{
    private Camera myCamera;

    void Awake()
    {
        myCamera = Camera.main;
    }
    public void OnDrop(PointerEventData eventData)
    {
        //When releasing a card on the sell part
        GameObject dropped = eventData.pointerDrag;
        if (dropped.GetComponent<Button>().interactable) {
            Vector3 pos = myCamera.ScreenToWorldPoint(Input.mousePosition);
            dropped.GetComponent<SpawnGhost>().spawnTheGhost(pos);
            if (dropped) {
                dropped.GetComponent<DraggableCard>().ResetPosition();
            }
        }
    }
}
