using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class listTargetNPC {
    public string[] tags;
    public int collect;
}

[CreateAssetMenu(fileName = "New NPCStats", menuName = "NPC")]
public class NPCStats : ScriptableObject
{
    public listTargetNPC[] tagTarget;
    public float[] coolDowncollect;
    public float rangecollect;
    public float rangecollectDetection;
    public float speed;
    public float hp;
    public int coinsDropped;
    public string soundDeath;
    public string soundSpawn;
}
