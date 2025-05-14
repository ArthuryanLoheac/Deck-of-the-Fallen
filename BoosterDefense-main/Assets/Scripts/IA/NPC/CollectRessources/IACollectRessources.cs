using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IACollectRessources : UniteIAClass
{
    public NPCStats mYstats;
    private float nextTimeAttack;
    private float coolDownAttack;
    
    IEnumerator delayCapacity(GameObject target, float animDuration)
    {
        yield return new WaitForSeconds(animDuration);
        if (target != null && !GetComponent<Life>().isDead){
            target.GetComponent<Ressource>().collectRessource(stats.tagTarget[getIdPriorityTarget(target)].collect, gameObject);
        }
    }

    public void Collect(GameObject target)
    {
        if (Time.time > nextTimeAttack) {
            animator.Play("Attack");
            StartCoroutine(delayCapacity(target, mYstats.getDelay("Attack")));
            nextTimeAttack = Time.time + ComputeSpeed(stats.coolDowncollect[0], false);
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
