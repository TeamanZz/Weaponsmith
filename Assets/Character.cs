using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [Header("Interactions settings")]
    public static Character character;
    public Camera dungeonCamera;

    public NavMeshAgent agent;

    public GameObject debugSphere;
    public float timeToTargetRemoval = 0.5f;

    [Header("New settings")]
    public Rigidbody rigidbody;
    public Animator animator;
    float walkVelocity = 0f;
    float attackVelocity = 0f;
    public float acceleratingSpeed = 1f;
    public float stopSpeed = 0.5f;
    int walkVelocityHash;
    int attackVelocityHash;

    public float stopDistance = 1f;
    public float agentStopDistance = 3f;
    public Transform cursor;

    public Vector3 targetPosition;
    public enum characterState
    {
        moveToPoint,
        attack
    }

    public void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        character = this;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        walkVelocityHash = Animator.StringToHash("WalkSpeed");
        attackVelocityHash = Animator.StringToHash("AttackSpeed");
        targetPosition = transform.position;
    }
    public void Update()
    {
        Ray ray = dungeonCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && Input.GetMouseButton(0))
        {
            GameObject target = Instantiate(debugSphere, hit.point, Quaternion.identity);
            Destroy(target, timeToTargetRemoval);
            
            float y = hit.point.y;
            Debug.DrawLine(transform.position, hit.transform.position);
            MoveToPoint(hit.point);
        }

        walkVelocity = rigidbody.velocity.magnitude;
        walkVelocity = Mathf.Clamp(walkVelocity, 0, 10f);

        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance < stopDistance)
            walkVelocity = 0;

        animator.SetFloat(walkVelocityHash, walkVelocity);

        //Debug.Log(distance);
    }
    public void MoveToPoint(Vector3 point)
    {
        float distance = Vector3.Distance(transform.position, point);
        //Debug.Log(distance);

        agent.SetDestination(point);
        targetPosition = point;
        //if (distance <= stopDistance)
        //{
        //    Debug.Log("Stop");
        //    //velocity = Mathf.MoveTowards(velocity, 0, stopSpeed * Time.deltaTime);
        //    velocity = 0;
        //    animator.SetFloat(velocityHash, velocity);

        //    animator.SetBool("Fight", false);
        //    agent.Stop();
        //    //return;
        //}
        //else
        //{
        //    //velocity = Mathf.MoveTowards(velocity, 1f, acceleratingSpeed * Time.deltaTime);
        //    animator.SetFloat(velocityHash, velocity);

        //    animator.SetBool("Fight", true);
        //    agent.Resume();
        //}
        
        //agent.stoppingDistance = agentStopDistance;

        //Debug.Log(agent.stoppingDistance);
        // Debug.Log(point);
        cursor.LookAt(point);
        float rot = cursor.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(transform.rotation.x, rot, transform.rotation.z);
        // Debug.Log(rot);

        Debug.DrawLine(transform.position, point, Color.red);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Enemy" && other.GetComponent<EnemyController>())
        {

        }
    }

    public void OnTriggerExit(Collider other)
    {
        
    }
}
