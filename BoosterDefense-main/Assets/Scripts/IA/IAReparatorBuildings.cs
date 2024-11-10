using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAReparatorBuildings : UniteIAClass
{
    public NPCStats mYstats;
    private float nextTimeAttack;
    private float coolDownAttack;
    
    public void Reparator(GameObject target)
    {
        if (Time.time > nextTimeAttack) {
            nextTimeAttack = Time.time + stats.coolDowncollect[0];
            target.GetComponent<Life>().TakeDamage(stats.tagTarget[getIdPriorityTarget(target)].collect, TypeDamage.Heal);
            animator.Play("Attack");
        }
    }
    public override void CapacitiesOnRange(GameObject target)
    {
        Reparator(target);
    }
    public override void CapacitiesPassives(){}
    public override void CapacitiesOnDeath(){}
    public override bool ChooseTarget(GameObject t) 
    {
        return t.GetComponent<Life>().hp < t.GetComponent<Life>().hpMax;
    }

    void Start()
    {
        StartSetValues(mYstats);
        coolDownAttack = coolDownCapacities[0];
        nextTimeAttack = coolDownAttack;
    }
}
