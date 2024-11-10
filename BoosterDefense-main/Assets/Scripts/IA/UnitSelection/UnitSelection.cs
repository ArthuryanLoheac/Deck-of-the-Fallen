using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public List<GameObject> unitlist = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();

    public static UnitSelection instance;

    private void AddUnit(GameObject unitClicked)
    {
        unitsSelected.Add(unitClicked);
        unitClicked.transform.GetChild(1).gameObject.SetActive(true);
    }
    private void RemoveUnit(GameObject unitClicked)
    {
        unitClicked.transform.GetChild(1).gameObject.SetActive(false);
        unitsSelected.Remove(unitClicked);
    }

    void Update() {
        unitlist.RemoveAll(x => x == null);
        unitsSelected.RemoveAll(x => x == null);
    }

    void Awake()
    {
        instance = this;
    }
    public void ClickSelect(GameObject unitClicked)
    {
        DeselectAll();
        AddUnit(unitClicked);
    }
    public void ShiftClickSelect(GameObject unitClicked)
    {
        if (!unitsSelected.Contains(unitClicked)) {
            AddUnit(unitClicked);
        } else {
            RemoveUnit(unitClicked);
        }
    }
    public void DragSelect(GameObject unitClicked)
    {
        if (!unitsSelected.Contains(unitClicked))
            AddUnit(unitClicked);
    }

    public void DeselectAll()
    {
        foreach(GameObject unit in unitsSelected) {
            unit.transform.GetChild(1).gameObject.SetActive(false);
        }
        unitsSelected.Clear();
    }
}
