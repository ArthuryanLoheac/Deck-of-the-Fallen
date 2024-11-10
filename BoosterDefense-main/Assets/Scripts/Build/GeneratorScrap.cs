using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorScrap : MonoBehaviour
{
    public float delayBetweenTwoSpawn;
    private float nexTimeSpawn;
    public int maxSpawn;
    private List<GameObject> lstObj;
    public List<GameObject> prefabs;
    public float range;
    public GameObject prefabUI;
    private Image ImageBar;
    private float sizeY;
    private GameObject RessourceBarGenerate;

    // Start is called before the first frame update
    void Start()
    {
        nexTimeSpawn = Time.time + delayBetweenTwoSpawn;
        lstObj = new List<GameObject>();

        sizeY = GetComponent<Collider>().bounds.size.y + 1f;
        Vector3 vec = new Vector3(transform.position.x, transform.position.y + sizeY + .5f, transform.position.z);

        RessourceBarGenerate = Instantiate(prefabUI, vec, Quaternion.identity, transform);
        ImageBar = RessourceBarGenerate.transform.GetChild(2).GetComponent<Image>();
    }

    private Vector3 GetRandomPos()
    {
        float dist = Random.Range(2f, range);
        Vector3 vec = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        while(Physics.Raycast(transform.position, vec, dist + 0.5f)) {
            vec = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        }
        return vec * dist;
    }

    private Quaternion randomRotation()
    {
        Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        return rotation;
    }

    // Update is called once per frame
    void Update()
    {
        lstObj.RemoveAll(x => x == null);
        if (Time.time >= nexTimeSpawn && lstObj.Count < maxSpawn) {
            lstObj.Add(Instantiate(prefabs[Random.Range(0, prefabs.Count)], transform.position + GetRandomPos(), randomRotation()));
            nexTimeSpawn = Time.time + delayBetweenTwoSpawn;
        }
        RessourceBarGenerate.SetActive(lstObj.Count < maxSpawn);
        ImageBar.fillAmount = (float)(nexTimeSpawn - Time.time) / (float)delayBetweenTwoSpawn;
    }
}
