using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIMove : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    float velocity = 0f;
    public float acceleratingSpeed = 1f;
    public float stopSpeed = 0.5f;
    int velocityHash;

    public float stopDistance = 1f;
    public float agentStopDistance = 3f;
    public Transform cursor;
    
    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("WalkSpeed");
    }

    public void MoveToPoint(Transform point)
    {
        float distance = Vector3.Distance(transform.position, point.transform.position);
        //Debug.Log(distance);

        agent.SetDestination(point.transform.position);

        if (distance <= stopDistance)
        {
            velocity = Mathf.MoveTowards(velocity, 0, stopSpeed * Time.deltaTime);
            animator.SetFloat(velocityHash, velocity);

            agent.Stop();
            return;
        }
        else
        {
            velocity = Mathf.MoveTowards(velocity, 1f, acceleratingSpeed * Time.deltaTime);
            animator.SetFloat(velocityHash, velocity);

            agent.Resume();
        }

        agent.stoppingDistance = agentStopDistance;

        Debug.Log(agent.stoppingDistance);
        // Debug.Log(point);
        cursor.LookAt(point); 
        float rot = cursor.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(transform.rotation.x, rot, transform.rotation.z);
       // Debug.Log(rot);

        Debug.DrawLine(transform.position, point.transform.position, Color.red);
    }

    public void AgentStop()
    {
        velocity = Mathf.MoveTowards(velocity, 0, stopSpeed * Time.deltaTime);
        animator.SetFloat(velocityHash, velocity);

        agent.Stop();
    }
}
