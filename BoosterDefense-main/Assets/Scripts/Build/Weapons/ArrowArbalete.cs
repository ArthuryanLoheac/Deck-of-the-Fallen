using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowArbalete : MonoBehaviour
{
    public float lifetime;
    public float speed;

    void Update()
    {
        //se déplace
        if (Time.time > lifetime)
            Destroy(gameObject);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    void Start()
    {
        lifetime += Time.time;
    }

    void OnTriggerEnter(Collider other)
    {
        //Se detruit au contact
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Plane"){
            Destroy(gameObject);
        }
    }
}
