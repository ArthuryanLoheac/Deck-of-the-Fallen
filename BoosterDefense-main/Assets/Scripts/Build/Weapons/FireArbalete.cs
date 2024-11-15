using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArbalete : MonoBehaviour
{
    public GameObject WeaponArm;
    public Weapon weapon;
    public float nextTimeShooting;
    private GameObject target;
    private GameObject ShootingPoint;
    private CoolDownActication ActivationManager;
    public float timeEffect = 2f;

    public GameObject FxBlood;
    public GameObject FxImpact;
    void Start()
    {
        ActivationManager = GetComponent<CoolDownActication>();
    }

    private void LookEnemy(GameObject enemy)
    {
        WeaponArm.transform.LookAt(enemy.transform.GetChild(0));
    }

    private void InfligeDamageToEennemy(GameObject enemy)
    {
        //Inflige les dégats dans X seconds
        if (enemy){
            enemy.GetComponent<Life>().TakeDamage(weapon.damage);
            enemy.GetComponent<BuffsAndDebuffs>().AddEffect(TypeBuffs.Fire, timeEffect, weapon.damage);
        }
    }

    private void ShootEnemy(GameObject enemy)
    {
        //Ataque enemies
        if (Time.time >= nextTimeShooting) {
            LookEnemy(enemy);
            nextTimeShooting = Time.time + weapon.cooldown;

            Vector3 positionEnemy = enemy.transform.position;
            positionEnemy.y += GetComponent<Collider>().bounds.size.y / 2;
            Vector3 directionVector = (positionEnemy - ShootingPoint.transform.position).normalized;

            RaycastHit hit;
            if (Physics.Raycast(ShootingPoint.transform.position, directionVector, out hit, weapon.range, LayerMask.GetMask("IAs")))
                Destroy(Instantiate(FxBlood, hit.point, Quaternion.FromToRotation(Vector3.zero, hit.normal)), 2f);
            if (Physics.Raycast(ShootingPoint.transform.position, directionVector, out hit, Mathf.Infinity, LayerMask.GetMask("BuildingLayer", "EnemyTerritory", "RessourceLayer", "Scraps", "Build")))
                Destroy(Instantiate(FxImpact, hit.point, Quaternion.FromToRotation(Vector3.zero, hit.normal)), 2f);

            InfligeDamageToEennemy(enemy);
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
