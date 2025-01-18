using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ressource : MonoBehaviour
{
    public int value;
    private int valueMax;
    public RessourceType typeRessource;
    private Image fillRessourceBar;
    private float sizeY;
    public GameObject CollectedRessourcePopup;

    void Update()
    {
        if (value <= 0)
            Destroy(gameObject);
        if (value < valueMax) {
            fillRessourceBar.transform.parent.parent.gameObject.SetActive(true);
        } else {
            fillRessourceBar.transform.parent.parent.gameObject.SetActive(false);
        }   
        fillRessourceBar.fillAmount = (float)value / (float)valueMax;
    }

    public void collectRessource(int amount, GameObject Collecter)
    {
        int valueCollected = Mathf.Min(amount, value);
        RessourceManager.instance.AddRessource(typeRessource, valueCollected);
        value -= valueCollected;
        Vector3 vec = new Vector3(Collecter.transform.position.x, Collecter.transform.position.y + Collecter.GetComponent<IACollectRessources>().sizeY + 0.5f, Collecter.transform.position.z);
        Instantiate(CollectedRessourcePopup, vec, Quaternion.identity).GetComponent<SetValue>().SetValueText(valueCollected, typeRessource);
    }

    public GameObject GetChildObject(Transform parent, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                return child.gameObject;
            }
            if (child.childCount > 0)
            {
                GetChildObject(child, _tag);
            }
        }
        return null;
    }

    void Start()
    {
        valueMax = value;

        sizeY = GetComponent<Collider>().bounds.size.y + 1f;
        Vector3 vec = new Vector3(transform.position.x, transform.position.y + sizeY + 0.5f, transform.position.z);

        GameObject statsBarGenerate = GetChildObject(transform, "StatsBarUi");
        if (statsBarGenerate == null) {
            statsBarGenerate = Instantiate(UIManager.instance.statsBar, vec, Quaternion.identity, transform);
        }
        fillRessourceBar = statsBarGenerate.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Image>();
    }
}
