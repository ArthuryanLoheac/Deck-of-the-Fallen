using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCursorCards : MonoBehaviour
{
    public void ButtonMoveCursor(int amount)
    {
        CardsManager.instance.MoveCursor(amount);
    }
}
