using UnityEngine;
using UnityEngine.AI;

public class UnitSelectMovement : MonoBehaviour
{
    Camera myCam;
    NavMeshAgent agent;
    private LayerMask Ground;
    Vector3 target;
    public float destinationReachedValue = 1f;
    public bool isPriorityMovement;

    // Start is called before the first frame update
    void Start()
    {
        myCam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        Ground = UnitClick.instance.Ground;
        target = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Ground)) {
                agent.SetDestination(hit.point);
                target = hit.point;
                isPriorityMovement = Input.GetKey(KeyCode.LeftShift);
            }
        }
    }
}
