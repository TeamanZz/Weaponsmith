using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCharacter : MonoBehaviour
{
    public float runSpeed;
    public DungeonEnemy currentEnemy;
    private Animator animator;

    public float scale = 1.5f;
    [Header("Battle settings")]
    public bool inBattle = false;
    public float distanceToTheEnemyToAttack = 0.5f;

    public float hitCooldownTime = 0.3f;
    public float currentCooldowneTime = 0f;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        transform.localScale = Vector3.one * scale; 
    }

    private void FixedUpdate()
    {
        if (PanelsHandler.currentLocationInTheDungeon == true)
        {
            //if (currentEnemy != null)
            //{
            //    float distance = Vector2.Distance(transform.position, currentEnemy.transform.position);
            //    if (distance < distanceToTheEnemyToAttack)
            //    {
            //        inBattle = true;
            //        Debug.Log("Distance = " + distance);
            //    }
            //}
          

            if (inBattle == false)
                transform.position += new Vector3(0, 0, runSpeed);
            else
                BattleControl();
        }
    }

    public void BattleControl()
    {
        if (currentEnemy == false)
        {
            inBattle = false;
            return;
        }
        
        if (currentCooldowneTime < 0 && inBattle == true)
        {
            Debug.Log("Hit");

            currentCooldowneTime = hitCooldownTime;
            animator.SetTrigger("EnemyIsNear");
            return;
             
        }
        else
            currentCooldowneTime -= Time.deltaTime;

    }
    private void OnTriggerEnter(Collider other)
    {
        DungeonEnemy tempEnemy;
        if (other.TryGetComponent<DungeonEnemy>(out tempEnemy))
        {
            //animator.SetTrigger("EnemyIsNear");
            inBattle = true;
            currentEnemy = tempEnemy;
        }
    }

    public void KillEnemy()
    {
        currentEnemy.InvokeDeathAnimation();
        inBattle = false;
    }
}