using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public GameObject coin;
    private float positionY;
    void OnMouseDown()
    {
        RessourceManager.instance.AddRessource(RessourceType.gold, GetComponent<Ressource>().value);
        Destroy(gameObject);
    }

    void Start()
    {
        positionY = coin.transform.position.y;
    }

    void Update()
    {
        coin.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * 50);
        coin.transform.position = new Vector3(coin.transform.position.x, positionY + Mathf.PingPong(Time.time / 20f, 0.1f), coin.transform.position.z);
    }
}
