using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownActication : MonoBehaviour
{
    public GameObject prefab;
    private Image ImageBar;
    public float TimeCooldown;
    private float TimeCooldownMax;
    public bool isActive = false;
    private float sizeY;

    void Start()
    {
        TimeCooldownMax = TimeCooldown;
        isActive = false;

        sizeY = GetComponent<Collider>().bounds.size.y + 1f;
        Vector3 vec = new Vector3(transform.position.x, transform.position.y + sizeY + .5f, transform.position.z);

        GameObject RessourceBarGenerate = Instantiate(prefab, vec, Quaternion.identity, transform);
        ImageBar = RessourceBarGenerate.transform.GetChild(2).GetComponent<Image>();
    }

    void Update()
    {
        //Updadte Cooldown
        if (TimeCooldown < 0) {
            isActive = true;
            ImageBar.transform.parent.gameObject.SetActive(false);
        } else {
            TimeCooldown -= Time.deltaTime;
            ImageBar.fillAmount = (float)TimeCooldown / (float)TimeCooldownMax;
            ImageBar.transform.parent.gameObject.SetActive(true);
        }
    }
}
