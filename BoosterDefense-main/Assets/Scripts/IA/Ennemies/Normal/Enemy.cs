using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : EnemyIAClass
{
    [Header("Stats")]
    public EnemyStats mYstats;
    private float nextTimeAttack;
    private float coolDownAttack;

    IEnumerator delayCapacity(GameObject target, float animDuration)
    {
        yield return new WaitForSeconds(animDuration);
        if (target != null && !GetComponent<Life>().isDead) {
            SoundManager.instance.PlaySound("ZombieAttackSimple");
            target.GetComponent<Life>().TakeDamage(stats.tagTarget[getIdPriorityTarget(target)].damage);
        }
    }
    public void Attack(GameObject targetAttack)
    {
        if (Time.time > nextTimeAttack) {
            nextTimeAttack = Time.time + ComputeSpeed(coolDownAttack, false);
            StartCoroutine(delayCapacity(targetAttack, mYstats.getDelay("Attack")));
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
        nextTimeAttack = coolDownAttack = stats.coolDownCapacities[0];
    }
}