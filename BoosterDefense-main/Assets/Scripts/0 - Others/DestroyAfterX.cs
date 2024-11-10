using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterX : MonoBehaviour
{
    public float X;

    void Start()
    {
        Destroy(gameObject, X);
    }
}
