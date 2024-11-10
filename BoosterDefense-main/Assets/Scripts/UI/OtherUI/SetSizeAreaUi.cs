using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSizeAreaUi : MonoBehaviour
{
    public void SetSize(float size)
    {
        Vector3 vec = transform.localScale;
        vec.x = size;
        vec.z = size;
        transform.localScale = vec;
    }
}
