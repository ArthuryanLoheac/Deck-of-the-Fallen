using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyExplodeOnDie : EnemyIAClass
{
    [Header("Explosion")]
    public GameObject Explosion;
    public float RadiusExplode;
    [Header("Stats")]
    public EnemyStats mYstats;
    private float nextTimeAttack;
    private float coolDownAttack;

    public void Attack(GameObject targetAttack)
    {
        if (Time.time > nextTimeAttack) {
            nextTimeAttack = Time.time + ComputeSpeed(coolDownAttack, false);
            targetAttack.GetComponent<Life>().TakeDamage(stats.tagTarget[getIdPriorityTarget(targetAttack)].damage);
            animator.Play("Attack");
            SoundManager.instance.PlaySound("ZombieAttackSimple");
        }
    }
    public void Explode()
    {
        if (Time.time > nextTimeAttack) {
            mYstats.soundDeath = "";
            SoundManager.instance.PlaySound("KamikazeBoom");
            animator.Play("Attack");
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, RadiusExplode);
            foreach(Collider col in hitColliders) {
                if (col.gameObject.GetComponent<Life>() && col.gameObject.tag != "Enemy")
                    col.gameObject.GetComponent<Life>().TakeDamage(stats.tagTarget[getIdPriorityTarget(col.gameObject)].damage, TypeDamage.Fire);
            }
            Instantiate(Explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    public override void CapacitiesOnRange(GameObject targetAttack)
    {
        Attack(targetAttack);
    }
    public override void CapacitiesPassives()
    {

    }
    public override void CapacitiesOnDeath()
    {
        Explode();
    }

    void Start()
    {
        StartSetValues(mYstats);
        nextTimeAttack = coolDownAttack = stats.coolDownCapacities[0];
    }
}