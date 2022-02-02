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
        targetPosition = transform.position;
        animator.SetBool("Fight", false);
    }
    public void Update()
    {
        Ray ray = dungeonCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && Input.GetMouseButton(0) && PanelsHandler.currentLocationInTheDungeon == true)
        {
            GameObject target = Instantiate(debugSphere, hit.point, Quaternion.identity);
            Destroy(target, timeToTargetRemoval);
            
            float y = hit.point.y;
            Debug.DrawLine(transform.position, hit.transform.position);
            MoveToPoint(hit.point);
        }

        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance < stopDistance && animator.GetBool("Fight") == true)
        {
            animator.SetBool("Fight", false);
            //Debug.Log(animator.GetBool("Fight"));
            //Debug.Log("Stoping " + stopDistance);
        }

        //Debug.Log(distance);
    }
    public void MoveToPoint(Vector3 point)
    {
        float distance = Vector3.Distance(transform.position, point);
        //Debug.Log(distance);

        agent.SetDestination(point);
        targetPosition = point;
        animator.SetBool("Fight", true);

        cursor.LookAt(point);
        float rot = cursor.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(transform.rotation.x, rot, transform.rotation.z);

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
