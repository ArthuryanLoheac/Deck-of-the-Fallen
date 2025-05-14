using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class listTarget {
    public string[] tags;
    public float damage;
}

[Serializable]
public class AnimationsDelayEnemy {
    public string animation;
    public float delay;
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
    public string soundDeath;
    public string soundSpawn;
    [Tooltip("Durée à attendre (en secondes) avant de collecter la ressource, afin de synchroniser avec l'animation.")]
    public AnimationsDelayEnemy[] syncWithAnimationDelay;

    public float getDelay(string anim)
    {
        foreach (AnimationsDelayEnemy ad in syncWithAnimationDelay) {
            if (ad.animation == anim) {
                return ad.delay;
            }
        }
        return 0;
    }
}
