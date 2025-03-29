using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UniteIAClass : MonoBehaviour
{
    [HideInInspector]public NPCStats stats;
    public GameObject target;
    [HideInInspector]private NavMeshAgent agent;
    [HideInInspector]public float pathUpdateDelay;
    [HideInInspector]public Animator animator;
    [HideInInspector]public float[] coolDownCapacities;
    [HideInInspector]public Life life;
    [HideInInspector]public float sizeY;
    [HideInInspector]public NavMeshPath path;

    [HideInInspector]public Camera myCam;
    [HideInInspector]public LayerMask Ground;
    public bool isPriorityMovement;
    [HideInInspector]public Vector3 positionPriorityMovement;
    [HideInInspector]public float previousValue = 0;
    [HideInInspector]public float yVelocity = 0.0f;
    [HideInInspector]public float nextTimeFindPath;

    #region Capacity
    public virtual void CapacitiesOnRange(GameObject targetAttack) {}
    public virtual void CapacitiesPassives() {}
    public virtual void CapacitiesOnDeath() {}
    public virtual bool ChooseTarget(GameObject t) { return true; }

    #endregion Capacity
    #region Update

    void Update()
    {
        if (!GetComponent<Life>().isDead){
            agent.speed = ComputeSpeed(stats.speed);
            CapacitiesPassives();
            if (isPriorityMovement) {
                //if priority movement -> move to this position
                PriorityMovement();
            } else {
                FindPathTimer();
                if (target != null) {
                    //if have a target -> check new one / move to his direction / etc
                    TargetManagement();
                } else {
                    //find a target
                    FindPathTimer();
                }
            }
            animator.SetFloat("Speed", SmoothValue(agent.desiredVelocity.sqrMagnitude));
            animator.SetFloat("SpeedAttack",  ComputeSpeed(1));
                CheckClickAndMove();
        }
    }
    #endregion  Update
    #region  IA

    void OnDestroy()
    {
        CapacitiesOnDeath();
    }    
    private void FindPathTimer()
    {
        if (Time.time > nextTimeFindPath) {
            nextTimeFindPath = Time.time + 1f;
            FindAndSetTarget();
        }
    }
    private void TargetManagement()
    {
        bool inRange = Vector3.Distance(transform.position, target.transform.position) <= stats.rangecollect + getBoudsSizeTarget() + 0.5f;

        if (inRange) {
            if (!ChooseTarget(target)) {
                FindAndSetTarget();
            } else {
                LookAtTarget();
                //---- Action when in range ----
                CapacitiesOnRange(target);
                //---- Action when in range ----
            }
        } else {
            UpdatePath();
        }
    }
    private float SmoothValue(float target)
    {
        previousValue = Mathf.SmoothDamp(previousValue, target, ref yVelocity, 0.1f);
        return previousValue;
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        life = GetComponent<Life>();
        
        myCam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    private void CheckClickAndMove()
    {
        if (Input.GetMouseButtonDown(1) && UnitSelection.instance.unitsSelected.Contains(gameObject)) {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Ground)) {
                agent.SetDestination(hit.point);
                isPriorityMovement = Input.GetKey(KeyCode.LeftShift);
                positionPriorityMovement = hit.point;
                target =  null;
            }
        }
    }

    private void PriorityMovement()
    {
        animator.SetBool("Attack", false);
        UpdatePathPriority();

        //Check is arrived
        if (Vector3.Distance(positionPriorityMovement, transform.position) <= agent.stoppingDistance) {
            isPriorityMovement = false;
            positionPriorityMovement = Vector3.zero;
            FindAndSetTarget();
        }
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

    public void StartSetValues(NPCStats mYstats)
    {
        stats = mYstats;
        coolDownCapacities = stats.coolDowncollect;
        Ground = UnitClick.instance.Ground;
        sizeY = GetComponent<Collider>().bounds.size.y + 0.5f;
        agent.speed = ComputeSpeed(stats.speed);
        agent.stoppingDistance = stats.rangecollect;
        life.hpMax = stats.hp;
        life.hp = stats.hp;
        life.coinsDropped = stats.coinsDropped;
        path = new NavMeshPath();
        animator.Play("Pop");
        SoundManager.instance.PlaySoundOneShot("SpawnNPC");
    }

    public int getIdPriorityTarget(GameObject obj)
    {
        string targetTag = obj.tag;
        int i = 0;
        
        foreach(listTargetNPC tags in stats.tagTarget) {
            foreach(string tag in tags.tags) {
                if (targetTag == tag)
                    return i;
            }
            i++;
        }
        return -1;
    }


    GameObject GetClosestTarget(List<GameObject> enemies)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in enemies)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist && ChooseTarget(t) && t.GetComponent<Life>().isDead == false)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    private List<GameObject> GetListObjectPriority(listTargetNPC tags)
    {
        List<GameObject> list = new List<GameObject>();
        foreach (string tag in tags.tags) {
            GameObject[] listTarget = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject target in listTarget) {
                if (Vector3.Distance(transform.position, target.transform.position) <= stats.rangecollectDetection)
                    list.Add(target);
            }
        }
        return list;
    }

    private GameObject FindTarget()
    {
        foreach(listTargetNPC tags in stats.tagTarget) {
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
            agent.stoppingDistance = stats.rangecollect - getBoudsSizeTarget();
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

    private void UpdatePath()
    {
        if (Time.time > pathUpdateDelay) {
            Vector3 lookPos = (target.transform.position - transform.position).normalized;
            agent.CalculatePath(target.transform.position - lookPos * (getBoudsSizeTarget() + 0.5f), path);
            agent.SetPath(path);
            pathUpdateDelay = Time.time + 0.2f;
        }
    }
    private void UpdatePathPriority()
    {
        if (Time.time > pathUpdateDelay) {
            agent.SetDestination(positionPriorityMovement);
            pathUpdateDelay = Time.time + 0.2f;
        }
    }
    #endregion  IA
}
