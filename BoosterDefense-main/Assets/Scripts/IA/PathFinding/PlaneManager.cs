using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    public static PlaneManager instance;

    public Transform min, max;
    public NavMeshSurface[] surfaces;

    void Awake()
    {
        instance = this;
    }

    public void BakeSurface()
    {
        foreach(NavMeshSurface plane in surfaces) {
            plane.BuildNavMesh();
        }
    }

    public Vector3 GetRandomPosOnPlane(Vector3 position, float range) {
        Vector3 vec = new Vector3(position.x + Random.Range(-range, range),
                            min.position.y,
                           position.z + Random.Range(-range, range));
        if (vec.x < min.position.x)
            vec.x = min.position.x;
        if (vec.z < min.position.z)
            vec.z = min.position.z;
        if (vec.x > max.position.x)
            vec.x = max.position.x;
        if (vec.z > max.position.z)
            vec.z = max.position.z;
        return vec;
    }

    void Start()
    {
        BakeSurface();
    }
}
