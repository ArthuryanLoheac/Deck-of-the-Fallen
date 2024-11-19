using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    public bool isBuilding = false;
    public Material validMaterial;
    public Material invalidMaterial;
    public LayerMask placementLayerMask;
    public LayerMask placementLayerMaskEnnemy;
    public GameObject FxBuild;

    void Awake()
    {
        instance = this;
    }
}
