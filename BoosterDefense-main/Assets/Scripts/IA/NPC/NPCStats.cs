using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class listTargetNPC {
    public string[] tags;
    public int collect;
}
[Serializable]
public class AnimationsDelayNPC {
    public string animation;
    public float delay;
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
    [Tooltip("Durée à attendre (en secondes) avant de collecter la ressource, afin de synchroniser avec l'animation.")]
    public AnimationsDelayNPC[] syncWithAnimationDelay;

    public float getDelay(string anim)
    {
        foreach (AnimationsDelayNPC ad in syncWithAnimationDelay) {
            if (ad.animation == anim) {
                return ad.delay;
            }
        }
        return 0;
    }
}
