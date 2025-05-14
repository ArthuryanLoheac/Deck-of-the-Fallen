 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;

public class placementInGrid : MonoBehaviour
{
    private Vector3 lastPos;
    //public bool clipToGrid = true;
    public GameObject objToSpawn;
    private bool isPosabled;
    public bool posableInEnnemyZone;
    public bool posableInHitBoxes = false;
    private float rotateAngle = 15;
    private float timer = 0.1f;

    private CardStats card;
    private int posInSiblings;
    public string soundSpawn;

    public void SetValues(CardStats cardToAdd, int pos)
    {
        posInSiblings = pos;
        card = cardToAdd;
    }

    // Update is called once per frame
    private void SetPositionOnGrid()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (!posableInEnnemyZone) {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, BuildManager.instance.placementLayerMask)) {
                lastPos = hit.point;
                transform.position = lastPos;
            }
        } else {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, BuildManager.instance.placementLayerMaskEnnemy)) {
                lastPos = hit.point;
                transform.position = lastPos;
            }
        }
    }

    private bool isCursorOnMap()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (!posableInEnnemyZone) {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, BuildManager.instance.placementLayerMask)) {
                return true;
            }
        }else {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, BuildManager.instance.placementLayerMaskEnnemy)) {
                return true;
            }
        }
        return false;
    }
    private void CheckPlaceObj()
    {
        if (Input.GetMouseButtonDown(0) && isCursorOnMap() && isPosabled && Time.time > timer) {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            SoundManager.instance.PlaySound(soundSpawn);
            BuildManager.instance.isBuilding = false;
            CardsManager.instance.UpdatePosCards();
            RessourceManager.instance.AddRessource(card.priceRessource, -card.price);
            if (card.addToCardUsed)
                CardsManager.instance.AddCardToCardUsed(card);
            Instantiate(objToSpawn, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if (Input.GetMouseButtonDown(1)) {
            BuildManager.instance.isBuilding = false;
            CardsManager.instance.AddCard(card);
            if (CardsManager.instance.GetCardInHand(card.name).cardCount == 1) {
                CardsManager.instance.GetCardInHand(card.name).transform.SetSiblingIndex(posInSiblings);
            }
            CardsManager.instance.UpdatePosCards();
            Destroy(gameObject);
        }
    }

    private void ChangeColors(Transform t, Material material)
    {
        for (int i = 0; i < t.childCount; i++) {
            if (t.GetChild(i).gameObject.GetComponent<Renderer>() != null && t.GetChild(i).gameObject.tag != "UiArea") {
                t.GetChild(i).gameObject.GetComponent<Renderer>().material = material;
            }
            if (t.GetChild(i).transform.childCount > 0) {
                ChangeColors(t.GetChild(i).transform, material);
            }
        }
    }

    private void CheckRotation()
    {
        float speed = rotateAngle;
        Vector3 pos = transform.eulerAngles;
        if (Input.GetKey(KeyCode.LeftShift))
            speed *= 3;
        pos.y += Input.mouseScrollDelta.y * speed;
        transform.eulerAngles = pos;

    }

    void Update()
    {
        SetPositionOnGrid();
        CheckPlaceObj();
        CheckRotation();

        //Changer la couleur de l'obj
        if (isPosabled) {
            ChangeColors(transform, BuildManager.instance.validMaterial);
        } else {
            ChangeColors(transform, BuildManager.instance.invalidMaterial);
        }
    }

    void Start()
    {
        isPosabled = true;
        timer += Time.time;
    }

    //ACTIVE OU DESACTIVE LA POSSIBILTE DE PLACER

    void OnTriggerEnter(Collider other)
    {
        if (posableInHitBoxes)
            isPosabled = true;
        else if (!(other.GetComponent<Ressource>() && other.GetComponent<Ressource>().typeRessource == RessourceType.goldInGame))
            isPosabled = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (posableInHitBoxes)
            isPosabled = true;
        else if (!(other.GetComponent<Ressource>() && other.GetComponent<Ressource>().typeRessource == RessourceType.goldInGame))
            isPosabled = false;
    }

    void OnTriggerExit(Collider other)
    {
        if (posableInHitBoxes)
            isPosabled = true;
        else if (!(other.GetComponent<Ressource>() && other.GetComponent<Ressource>().typeRessource == RessourceType.goldInGame))
            isPosabled = true;
    }
}
