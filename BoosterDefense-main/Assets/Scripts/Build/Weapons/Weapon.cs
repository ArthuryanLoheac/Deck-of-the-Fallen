using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public int damage;
    public int range;
    public float cooldown;
    public TypeBuffs typeBuffs;
    public float damageBuffs;
    public float timeEffect;
}
