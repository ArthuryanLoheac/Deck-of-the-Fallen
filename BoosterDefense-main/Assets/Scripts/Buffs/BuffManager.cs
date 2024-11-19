using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    //    -1 pour passifs 
    public static BuffManager instance;
    public float cooldownHeal;
    public float cooldownFire;
    public float cooldownInvincibility = -1;
    public float cooldownSlow = -1;
    // force 5 = -5% vitesse
    public GameObject FX_Fire;

    public float GetCoolDown(TypeBuffs type)
    {
        switch(type) {
            case TypeBuffs.Fire:
                return cooldownFire;
            case TypeBuffs.Heal:
                return cooldownHeal;
            case TypeBuffs.Invincibility:
                return cooldownInvincibility;
            case TypeBuffs.Slow:
                return cooldownSlow;
        }
        return 1;
    }
    public TypeMore GetAddType(TypeBuffs type)
    {
        switch(type) {
            case TypeBuffs.Slow:
                return TypeMore.Reset;
            case TypeBuffs.Fire:
                return TypeMore.Reset;
            case TypeBuffs.Invincibility:
                return TypeMore.Reset;
            default:
                return TypeMore.Add;
        }
    }

    void Awake()
    {
        instance = this;
    }
}
