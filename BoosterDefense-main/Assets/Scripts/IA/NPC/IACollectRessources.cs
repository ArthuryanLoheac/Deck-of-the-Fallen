using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IACollectRessources : UniteIAClass
{
    public NPCStats mYstats;
    private float nextTimeAttack;
    private float coolDownAttack;
    
    public void Collect(GameObject target)
    {
        if (Time.time > nextTimeAttack) {
            nextTimeAttack = Time.time + ComputeSpeed(stats.coolDowncollect[0], false);
            target.GetComponent<Ressource>().collectRessource(stats.tagTarget[getIdPriorityTarget(target)].collect, gameObject);
            animator.Play("Attack");
        }
    }
    public override void CapacitiesOnRange(GameObject target)
    {
        Collect(target);
    }
    public override void CapacitiesPassives(){}
    public override void CapacitiesOnDeath(){}

    void Start()
    {
        StartSetValues(mYstats);
        coolDownAttack = coolDownCapacities[0];
        nextTimeAttack = coolDownAttack;
    }
}
