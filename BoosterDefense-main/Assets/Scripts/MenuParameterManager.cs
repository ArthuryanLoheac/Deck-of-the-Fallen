using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuParameterManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ParameterManager.instance.OpenCloseParameters(!ParameterManager.instance.isOpen);
        }
    }
}
