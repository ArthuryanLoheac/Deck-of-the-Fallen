using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    // Update is called once per frame
    void Start()
    {
        transform.LookAt(transform.position - Camera.main.transform.position);
    }
    void Update()
    {
        transform.LookAt(transform.position - Camera.main.transform.position);       
    }
}
