using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : MonoBehaviour
{
    public float RangeExplosion = 1;
    public float timeEffect = 1;
    public float Damage = 1;

    private float TimeEnd = 0f;
    public GameObject FireFx;

    IEnumerator StopAfterDelay(GameObject obj)
    {
        float times = Random.Range(0.5f, 2f);
        yield return new WaitForSeconds(timeEffect - times);
        obj.GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(times);
        Destroy(obj, timeEffect + 1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        TimeEnd = Time.time + timeEffect;

        for (int i = 0; i < RangeExplosion * 30; i++)
        {
            float range = Random.Range(0f, RangeExplosion);
            float angle = Random.Range(0f, 360f);

            Vector2 vec = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
            vec = vec.normalized;
            Vector3 pos = new Vector3(transform.position.x + (vec.x * range), transform.position.y, transform.position.z + (vec.y * range));

            GameObject obj = Instantiate(FireFx, pos, Quaternion.identity, transform);
            StartCoroutine(StopAfterDelay(obj));
        }
    }

    void Update()
    {
        if (Time.time <= TimeEnd) { 
            Collider[] cols = Physics.OverlapSphere(transform.position, RangeExplosion, RessourceManager.instance.AllLayer);
            foreach (Collider c in cols)
            {
                if (c.gameObject.tag == "Enemy" || c.gameObject.tag == "NPC" || c.gameObject.tag == "Food")
                {
                    c.gameObject.GetComponent<BuffsAndDebuffs>().AddEffect(TypeBuffs.Fire, timeEffect, Damage);
                }
            }
        } else { 
            Destroy(this.gameObject, 1f);
        }
    }
}
