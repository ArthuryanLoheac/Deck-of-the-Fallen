using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenParameter : MonoBehaviour
{
    public void OpenParams()
    {
        ParameterManager.instance.OpenCloseParameters(true);
    }
    public void CloseParams()
    {
        ParameterManager.instance.OpenCloseParameters(false);
    }
}
