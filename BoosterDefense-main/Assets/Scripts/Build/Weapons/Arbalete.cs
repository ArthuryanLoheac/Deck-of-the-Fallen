using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbalete : MonoBehaviour
{
    public GameObject WeaponArm;
    public Weapon weapon;
    public float nextTimeShooting;
    private GameObject target;
    public GameObject arrow;
    private CoolDownActication ActivationManager;

    void Start()
    {
        ActivationManager = GetComponent<CoolDownActication>();
    }

    private void LookEnemy(GameObject enemy)
    {
        var targetRotation = Quaternion.LookRotation(enemy.transform.GetChild(0).position - WeaponArm.transform.position);
        WeaponArm.transform.rotation = Quaternion.Slerp(WeaponArm.transform.rotation, targetRotation, 10 * Time.deltaTime);
    }

    private IEnumerator InfligeDamageToEennemy(GameObject enemy, float time)
    {
        //Inflige les dégats dans X seconds
        yield return new WaitForSeconds(time);
        if (enemy)
            enemy.GetComponent<Life>().TakeDamage(weapon.damage);
    }

    private void ShootEnemy(GameObject enemy)
    {
        //Ataque enemies
        if (Time.time >= nextTimeShooting) {
            LookEnemy(enemy);
            nextTimeShooting = Time.time + weapon.cooldown;
            Vector3 vec = WeaponArm.transform.position;
            vec.y *= 1.5f;
            GameObject ar = Instantiate(arrow, vec, WeaponArm.transform.rotation, transform);

            float time = Vector3.Distance(transform.position, enemy.transform.position) / ar.GetComponent<ArrowArbalete>().speed;
            StartCoroutine(InfligeDamageToEennemy(enemy, time));
        }
    }

    private void UpdateNoTarget()
    {
        //Cherche target
        Collider[] cols = Physics.OverlapSphere(transform.position, weapon.range, RessourceManager.instance.AllLayer);
        
        foreach (Collider col in cols) {
            if (col.tag == "Enemy") {
                target = col.gameObject;
                ShootEnemy(target);
            }
        }
    }

    private void UpdateTarget()
    {
        //Check target toujours à distance
        if (Vector3.Distance(target.transform.position, transform.position) > weapon.range) {
            target = null;
        } else {
            LookEnemy(target);
            ShootEnemy(target);
        }
    }

    void Update()
    {
        if (ActivationManager.isActive){
            //si le delay est passé cherche target ou attack
            if (target == null) {
                UpdateNoTarget();
            }
            if (target != null) {
                UpdateTarget();
            }
        }
    }
}
