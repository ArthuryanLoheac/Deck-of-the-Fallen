using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("SoundManager");
        SoundManager.instance.PlaySound("StartMenu", true);
    }
}
