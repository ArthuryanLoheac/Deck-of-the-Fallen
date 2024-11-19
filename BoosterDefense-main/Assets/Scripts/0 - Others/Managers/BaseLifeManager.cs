using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLifeManager : MonoBehaviour
{
    public static BaseLifeManager instance;
    public float life = 0;
    public float lifeMax = 0;
    
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    public float getPercentHealth()
    {
        return life / lifeMax;
    }
}
