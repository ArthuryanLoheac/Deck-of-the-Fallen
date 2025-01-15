using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitDrag : MonoBehaviour
{
    public static UnitDrag instance;
    private  Camera myCamera;
    
    [SerializeField] private RectTransform boxVisual;
    private Rect selectionBox;

    private Vector2 StartPosition;
    private Vector2 EndPosition;
    public bool IsDraggingCard;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main;
        StartPosition = Vector2.zero;
        EndPosition = Vector2.zero;
        DrawVisual();
    }

    // Update is called once per frame
    void Update()
    {
        //click
        if (Input.GetMouseButtonDown(0) && !IsDraggingCard) {
            StartPosition = Input.mousePosition;
            selectionBox = new Rect();
        }

        //drag
        if (Input.GetMouseButton(0) && !IsDraggingCard) {
            EndPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        //end
        if (Input.GetMouseButtonUp(0) && !IsDraggingCard) {
            SelectUnits();
            StartPosition = Vector2.zero;
            EndPosition = Vector2.zero;
            DrawVisual();
        }
    }

    void DrawVisual()
    {
        //Set size of the box
        Vector2 boxStart = StartPosition;
        Vector2 boxEnd = EndPosition;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        if (Input.mousePosition.x < StartPosition.x) {
            //drag left
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = StartPosition.x;
        } else {
            //drag right
            selectionBox.xMin = StartPosition.x;
            selectionBox.xMax = Input.mousePosition.x;
        }

        if (Input.mousePosition.y < StartPosition.y) {
            //drag down
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = StartPosition.y;
        } else {
            //drag up
            selectionBox.yMin = StartPosition.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
        
    }
    void SelectUnits()
    {
        //foreach unit in map
        foreach(var unit in UnitSelection.instance.unitlist) {
            //check if in  the rect
            if (selectionBox.Contains(myCamera.WorldToScreenPoint(unit.transform.position))) {
                UnitSelection.instance.DragSelect(unit);
            }
        }
    }
}
