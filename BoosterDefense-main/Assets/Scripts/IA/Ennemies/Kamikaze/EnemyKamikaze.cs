using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyKamikaze : EnemyIAClass
{

    [Header("Explosion")]
    public GameObject Explosion;
    public float RadiusExplode;
    [Header("Stats")]
    public EnemyStats mYstats;
    private float nextTimeAttack;
    private float coolDownAttack;

    IEnumerator delayCapacityExplode(float animDuration)
    {
        yield return new WaitForSeconds(animDuration);
        if (!GetComponent<Life>().isDead) {
            SoundManager.instance.PlaySound("KamikazeBoom");
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, RadiusExplode);
            foreach(Collider col in hitColliders) {
                if (col.gameObject.GetComponent<Life>() && col.gameObject.tag != "Enemy")
                    col.gameObject.GetComponent<Life>().TakeDamage(stats.tagTarget[getIdPriorityTarget(col.gameObject)].damage, TypeDamage.Fire);
            }
            Instantiate(Explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public void Explode()
    {
        if (Time.time > nextTimeAttack) {
            mYstats.soundDeath = "";
            animator.Play("Attack");
            StartCoroutine(delayCapacityExplode(mYstats.getDelay("Attack")));
        }
    }
    public override void CapacitiesOnRange(GameObject targetAttack)
    {
        Explode();
    }
    public override void CapacitiesPassives(){}
    public override void CapacitiesOnDeath(){}

    void Start()
    {
        StartSetValues(mYstats);
        nextTimeAttack = coolDownAttack = stats.coolDownCapacities[0];
    }
}