using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCharacter : MonoBehaviour
{
    public float runSpeed;
    public DungeonEnemy currentEnemy;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(PanelsHandler.currentLocationInTheDungeon == true)
            transform.position += new Vector3(0, 0, runSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        DungeonEnemy tempEnemy;
        if (other.TryGetComponent<DungeonEnemy>(out tempEnemy))
        {
            animator.SetTrigger("EnemyIsNear");
            currentEnemy = tempEnemy;
        }
    }

    public void KillEnemy()
    {
        currentEnemy.InvokeDeathAnimation();
    }
}