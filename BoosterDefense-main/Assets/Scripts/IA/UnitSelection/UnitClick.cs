using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitClick : MonoBehaviour
{
    private Camera myCamera;
    public LayerMask Clickable;
    public LayerMask Ground;
    public static UnitClick instance;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Clickable)) {
                //hit an object
                if (Input.GetKey(KeyCode.LeftShift)) {
                    UnitSelection.instance.ShiftClickSelect(hit.collider.gameObject);
                    //shift select
                } else {
                    UnitSelection.instance.ClickSelect(hit.collider.gameObject);
                    //normal select
                }
            } else {
                //click elsewhere
                if (!Input.GetKey(KeyCode.LeftShift))
                    UnitSelection.instance.DeselectAll();
            }
        }
    }
}
