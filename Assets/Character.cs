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

    public float stopingDistance = 2f;
    public float agentStopDistance = 3f;
    public Transform cursor;

    public Vector3 targetPosition;
    [Header("Battl settings")]
    public List<Enemy> enemies = new List<Enemy>();
    public bool inAttack = false;
    public float hitReload = 0.4f;
    public enum characterState
    {
        moveToPoint,
        attack
    }

    public Vector3 enemyPosition;
        void Awake()
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

        if(enemies.Count > 0 && inAttack == false)
        {
            AttackEnemy(enemies[0]);
            inAttack = true;
        }

        if (inAttack == true && enemyPosition != null)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemyPosition);
            if(distanceToEnemy <= stopingDistance)
            {
                animator.SetBool("Fight", false);
            }
        }

        if (Physics.Raycast(ray, out hit) && Input.GetMouseButton(0) && PanelsHandler.currentLocationInTheDungeon == true && inAttack == false)
        {
            GameObject target = Instantiate(debugSphere, hit.point, Quaternion.identity);
            Destroy(target, timeToTargetRemoval);
            
            float y = hit.point.y;
            Debug.DrawLine(transform.position, hit.transform.position);
            MoveToPoint(hit.point);
        }

        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance < stopingDistance && animator.GetBool("Fight") == true)
        {
            animator.SetBool("Fight", false);
            //Debug.Log(animator.GetBool("Fight"));
            //Debug.Log("Stoping " + stopDistance);
        }

        //Debug.Log(distance);
    }

    public void AttackEnemy(Enemy enemy)
    {
        animator.SetBool("Fight", true);

        agent.SetDestination(enemy.transform.position);
        agent.stoppingDistance = stopingDistance;
    }
    public void MoveToPoint(Vector3 point)
    {
        float distance = Vector3.Distance(transform.position, point);
        //Debug.Log(distance);

        var path = new NavMeshPath();
        agent.CalculatePath(point, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(point);
            targetPosition = point;
            cursor.LookAt(point);

            animator.SetBool("Fight", true);

            float rot = cursor.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(transform.rotation.x, rot, transform.rotation.z);

            Debug.DrawLine(transform.position, point, Color.red);
            agent.Resume();
        }
        else
        {
            animator.SetBool("Fight", false);

            Debug.Log("Path false");
            agent.Stop();
        }
    }

    
}
