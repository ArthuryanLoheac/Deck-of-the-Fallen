using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIAClass : MonoBehaviour
{
    [Header("Stats")]
    [HideInInspector]public EnemyStats stats;
    [HideInInspector]public Life life;

    [HideInInspector]public GameObject target;
    [HideInInspector]public float speedComputed;
    [HideInInspector]public NavMeshAgent agent;
    [HideInInspector]public float pathUpdateDelay;
    [HideInInspector]public NavMeshPath path;
    [HideInInspector]public float nextTimeUpdateRandomPos;
    [HideInInspector]public Vector3 randomPos;
    [HideInInspector]public float timeActiveMove;

    [HideInInspector]public Animator animator;
    [HideInInspector]public float previousValue = 0;
    [HideInInspector]public float yVelocity = 0.0f;
    [HideInInspector]public float nextTimeFindPath;

    #region Capacity
    public virtual void CapacitiesOnRange(GameObject targetAttack) {}
    public virtual void CapacitiesPassives() {}
    public virtual void CapacitiesOnDeath() {}
    #endregion Capacity
    #region Update
    void Update()
    {
        if (!GetComponent<Life>().isDead)
        {
            agent.speed = ComputeSpeed(stats.speed);
            if (Time.time > timeActiveMove)
            {
                FindPathTimer(); // cherche chemin tout les X
                if (target != null)
                {
                    bool inRange = Vector3.Distance(transform.position, target.transform.position) <= stats.range + getBoudsSizeTarget() + 0.5f;

                    if (inRange)
                    {
                        LookAtTarget();
                        CapacitiesOnRange(target);
                    }
                    else
                    {
                        UpdatePath();
                        CheckBlockingObject();
                    }
                }
                else
                {
                    FindPathTimer();
                }
                animator.SetFloat("Speed", Mathf.Min(1, SmoothValue(agent.desiredVelocity.sqrMagnitude)));
                animator.SetFloat("SpeedAttack", ComputeSpeed(1));
                CapacitiesPassives();
            }
        }
    }
    void OnDestroy()
    {
        CapacitiesOnDeath();
    }
    #endregion Update
    #region IA
    private void FindPathTimer()
    {
        if (Time.time > nextTimeFindPath) {
            nextTimeFindPath = Time.time + 1f;
            FindAndSetTarget();
        }
    }
    private float SmoothValue(float target)
    {
        previousValue = Mathf.SmoothDamp(previousValue, target, ref yVelocity, 0.2f);
        return previousValue;
    }
    private bool CheckBlockingObject()
    {
        //Attaque l'objet devant si il bloque
        if (agent.desiredVelocity.sqrMagnitude < 0.1f) {
            RaycastHit objectHit;
            Vector3 start = transform.position;
            start.y += 0.5f;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
    
            if (Physics.Raycast(start, fwd, out objectHit, 1)) {
                if(objectHit.collider.gameObject.tag !="Enemy"){
                    CapacitiesOnRange(objectHit.collider.gameObject);
                    return true;
                }
            }
        }
        return false;
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        life = GetComponent<Life>();
    }

    public float ComputeSpeed(float speedValue, bool isReduce=true)
    {
        float speed = speedValue;
        if (GetComponent<BuffsAndDebuffs>().isBuffActive(TypeBuffs.Slow)) {
            Buffs buff = GetComponent<BuffsAndDebuffs>().GetFirstBuffActive(TypeBuffs.Slow);
            if (isReduce)
                speed -= speed / 100 * buff.force;
            else
                speed += speed / 100 * buff.force;
        }
        return speed;
    }

    public void StartSetValues(EnemyStats mYstats)
    {
        stats = mYstats;
        //Set stats
        agent.speed = ComputeSpeed(stats.speed);
        agent.stoppingDistance = stats.range;
        life.hpMax = stats.hp;
        life.hp = stats.hp;
        life.coinsDropped = stats.coinsDropped;

        path = new NavMeshPath();
        animator.Play("WakeUp");
        timeActiveMove = Time.time + 2.4f;
    }

    public int getIdPriorityTarget(GameObject obj)
    {
        //cherche target
        string targetTag = obj.tag;
        int i = 0;
        
        foreach(listTarget tags in stats.tagTarget) {
            foreach(string tag in tags.tags) {
                if (targetTag == tag)
                    return i;
            }
            i++;
        }
        return stats.tagTarget.Length - 1;
    }

    GameObject GetClosestTarget(List<GameObject> enemies)
    {
        //Target la plus proche
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in enemies)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist && t.GetComponent<Life>().isDead == false)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    private List<GameObject> GetListObjectPriority(listTarget tags)
    {
        List<GameObject> list = new List<GameObject>();
        foreach (string tag in tags.tags) {
            GameObject[] listTarget = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject target in listTarget) {
                list.Add(target);
            }
        }
        return list;
    }

    private GameObject FindTarget()
    {
        foreach(listTarget tags in stats.tagTarget) {
            List<GameObject> list = GetListObjectPriority(tags);
            if (list.Count > 0) {
                return GetClosestTarget(list);
            }
        }
        return null;
    }

    private void FindAndSetTarget()
    {
        GameObject targetFound = FindTarget();
        if (targetFound) {
            target = targetFound;
            agent.stoppingDistance = stats.range - getBoudsSizeTarget();
        } else {
            animator.SetBool("Attack", false);
        }
    }

    private void LookAtTarget()
    {
        Vector3 lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }

    private float getBoudsSizeTarget()
    {
        if (target)
            return (target.GetComponent<Collider>().bounds.size.x / 2 +
                target.GetComponent<Collider>().bounds.size.z / 2) / 2;
        else
            return 0;
    }

    private void ComputePath()
    {
        Vector3 lookPos = (target.transform.position - transform.position).normalized;
        if (agent.CalculatePath(target.transform.position - lookPos * (getBoudsSizeTarget() + 0.5f), path))
            return;
        if (agent.CalculatePath(target.transform.position - (-lookPos) * (getBoudsSizeTarget() + 0.5f), path))
            return;
        Vector3 newlookPos = new Vector3 (lookPos.z, lookPos.y, lookPos.x);
        if (agent.CalculatePath(target.transform.position - newlookPos * (getBoudsSizeTarget() + 0.5f), path))
            return;
        if (agent.CalculatePath(target.transform.position - (-newlookPos) * (getBoudsSizeTarget() + 0.5f), path))
            return;
        while(!agent.CalculatePath(randomPos, path)) {
            randomPos = PlaneManager.instance.GetRandomPosOnPlane(transform.position, 10f);
        }
    }

    private void UpdatePath()
    {
        if (Time.time > nextTimeUpdateRandomPos) {
            randomPos = PlaneManager.instance.GetRandomPosOnPlane(transform.position, 10f);
        }
        if (Time.time > pathUpdateDelay) {
            ComputePath();
            agent.SetPath(path);
            pathUpdateDelay = Time.time + 1f;
        }
    }
    #endregion IA
}