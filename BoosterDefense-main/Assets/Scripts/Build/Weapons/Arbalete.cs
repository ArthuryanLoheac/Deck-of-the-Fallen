using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbalete : MonoBehaviour
{
    public Weapon weapon;
    public float nextTimeShooting;
    private GameObject target;
    private CoolDownActication ActivationManager;
    public GameObject FxShoot;
    public GameObject ShootingPoint;

    public GameObject FxBlood;
    public GameObject FxImpact;


    void Start()
    {
        ActivationManager = GetComponent<CoolDownActication>();
    }

    private void InfligeDamageToEennemy(GameObject enemy)
    {
        //Inflige les dégats dans X seconds
        if (enemy)
            enemy.GetComponent<Life>().TakeDamage(weapon.damage);
    }

    private void ShootEnemy(GameObject enemy)
    {
        //Ataque enemies
        if (Time.time >= nextTimeShooting) {
            nextTimeShooting = Time.time + weapon.cooldown;

            Vector3 positionEnemy = enemy.transform.position;
            positionEnemy.y += GetComponent<Collider>().bounds.size.y / 2;
            Vector3 directionVector = (positionEnemy - ShootingPoint.transform.position).normalized;

            GameObject obj = Instantiate(FxShoot, ShootingPoint.transform.position, Quaternion.LookRotation(directionVector), ShootingPoint.transform);

            RaycastHit hit;
            if (Physics.Raycast(ShootingPoint.transform.position, directionVector, out hit, weapon.range, LayerMask.GetMask("IAs")))
                Destroy(Instantiate(FxBlood, hit.point, Quaternion.FromToRotation(Vector3.zero, hit.normal)), 2f);
            if (Physics.Raycast(ShootingPoint.transform.position, directionVector, out hit, Mathf.Infinity, LayerMask.GetMask("BuildingLayer", "EnemyTerritory", "RessourceLayer", "Scraps", "Build")))
                Destroy(Instantiate(FxImpact, hit.point, Quaternion.FromToRotation(Vector3.zero, hit.normal)), 2f);
         
            Destroy(obj, 2f);
            InfligeDamageToEennemy(enemy);
        }
    }

    private void UpdateNoTarget()
    {
        //Cherche target
        Collider[] cols = Physics.OverlapSphere(transform.position, weapon.range, RessourceManager.instance.AllLayer);
        
        foreach (Collider col in cols) {
            if (col.tag == "Enemy" && !col.gameObject.GetComponent<Life>().isDead) {
                target = col.gameObject;
                ShootEnemy(target);
            }
        }
    }

    private void UpdateTarget()
    {
        //Check target toujours à distance
        if (Vector3.Distance(target.transform.position, transform.position) > weapon.range || target.GetComponent<Life>().isDead) {
            target = null;
        } else {
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
