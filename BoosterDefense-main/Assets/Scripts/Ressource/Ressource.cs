using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ressource : MonoBehaviour
{
    public int value;
    private int valueMax;
    public RessourceType typeRessource;
    public GameObject RessourceBar;
    private Image fillHpBar;
    private float sizeY;
    public GameObject CollectedRessourcePopup;

    void Update()
    {
        if (value <= 0)
            Destroy(gameObject);
        if (value < valueMax) {
            fillHpBar.transform.parent.gameObject.SetActive(true);
        } else {
            fillHpBar.transform.parent.gameObject.SetActive(false);
        }   
        fillHpBar.fillAmount = (float)value / (float)valueMax;
    }

    public void collectRessource(int amount, GameObject Collecter)
    {
        int valueCollected = Mathf.Min(amount, value);
        RessourceManager.instance.AddRessource(typeRessource, valueCollected);
        value -= valueCollected;
        Vector3 vec = new Vector3(Collecter.transform.position.x, Collecter.transform.position.y + Collecter.GetComponent<IACollectRessources>().sizeY + 0.5f, Collecter.transform.position.z);
        Instantiate(CollectedRessourcePopup, vec, Quaternion.identity).GetComponent<SetValue>().SetValueText(valueCollected, typeRessource);
    }

    void Start()
    {
        valueMax = value;

        sizeY = GetComponent<Collider>().bounds.size.y + 1f;
        Vector3 vec = new Vector3(transform.position.x, transform.position.y + sizeY + 0.5f, transform.position.z);

        GameObject RessourceBarGenerate = Instantiate(RessourceBar, vec, Quaternion.identity, transform);
        fillHpBar = RessourceBarGenerate.transform.GetChild(2).GetComponent<Image>();
    }
}
