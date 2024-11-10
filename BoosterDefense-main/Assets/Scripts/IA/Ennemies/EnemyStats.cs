using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class listTarget {
    public string[] tags;
    public float damage;
}

[CreateAssetMenu(fileName = "New EnemyStats", menuName = "Enemy")]
public class EnemyStats : ScriptableObject
{
    public listTarget[] tagTarget;
    public float damage;
    public float[] coolDownCapacities;
    public float range;
    public float speed;
    public float hp;
    public int coinsDropped;
    public float zone;
}
