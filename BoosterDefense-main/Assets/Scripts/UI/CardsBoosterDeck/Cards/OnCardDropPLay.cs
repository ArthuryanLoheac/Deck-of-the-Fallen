using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnCardDropPLay : MonoBehaviour, IDropHandler
{
    public static OnCardDropPLay instance;
    private Camera myCamera;
    public Image img;

    void Awake()
    {
        instance = this;
        myCamera = Camera.main;
    }

    void Start()
    {
        img.raycastTarget = false;
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("ON DROP :");
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

    public void startDrag()
    {
        img.raycastTarget = true;
    }
    public void endDrag()
    {
        img.raycastTarget = false;
    }
}
