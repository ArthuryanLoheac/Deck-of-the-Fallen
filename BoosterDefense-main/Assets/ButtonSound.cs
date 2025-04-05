using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GetComponent<Button>() != null && GetComponent<Button>().interactable == false) return;
        SoundManager.instance.PlaySound("ButtonClick");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponent<Button>() != null && GetComponent<Button>().interactable == false) return;
        SoundManager.instance.PlaySound("ButtonHover");
    }
}
