using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAAttackMonster : UniteIAClass
{
    public NPCStats mYstats;
    private float nextTimeAttack;
    private float coolDownAttack;

    
    public void Attack(GameObject targetAttack)
    {
        if (Time.time > nextTimeAttack) {
            nextTimeAttack = Time.time + ComputeSpeed(coolDownAttack, false);
            targetAttack.GetComponent<Life>().TakeDamage(stats.tagTarget[getIdPriorityTarget(targetAttack)].collect);
            animator.Play("Attack");
        }
    }
    public override void CapacitiesOnRange(GameObject targetAttack)
    {
        Attack(targetAttack);
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
