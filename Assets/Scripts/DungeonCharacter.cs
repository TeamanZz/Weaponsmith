using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonCharacter : MonoBehaviour
{
    public static DungeonCharacter dungeonCharacter;
    public float runSpeed;
    public DungeonEnemy currentEnemy;
    public Animator animator;

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
       // enemyHealthBar.SetActive(false);
    }
    public void Start()
    {
        dungeonCharacter = this;
    }
    private void FixedUpdate()
    {
        if (PanelsHandler.currentLocationInTheDungeon == true)
        {
            if (inBattle == false)
            {
                if (EnemyHealthBar.enemyHealthBarController.isInitialization == true || EnemyHealthBar.enemyHealthBarController.isOpen == true)
                    EnemyHealthBar.enemyHealthBarController.Deinitialization();

                    transform.position += new Vector3(0, 0, runSpeed);
            }
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
            //Debug.Log("Hit");

            currentCooldowneTime = hitCooldownTime;

            if (EnemyHealthBar.enemyHealthBarController.isInitialization == false)
                EnemyHealthBar.enemyHealthBarController.Initialization(currentEnemy);

            EnemyHealthBar.enemyHealthBarController.OpenHealthBar();

            animator.SetTrigger("EnemyIsNear");
            return;
             
        }
        else
            currentCooldowneTime -= Time.deltaTime;

    }
    public void TakeDamageControll()
    {
        //Invoke("EnemyHealthBar.enemyHealthBarController.TakeDamageControll", 0.3f);
        Invoke(() => EnemyHealthBar.enemyHealthBarController.TakeDamageControll(), 0.05f);
        //EnemyHealthBar.enemyHealthBarController.TakeDamageControll();
    }

    private void Invoke(Action p, float v)
    {
        EnemyHealthBar.enemyHealthBarController.TakeDamageControll();
    }

    private void OnTriggerEnter(Collider other)
    {
        DungeonEnemy tempEnemy;
        if (other.TryGetComponent<DungeonEnemy>(out tempEnemy))
        {
            //animator.SetTrigger("EnemyIsNear");
            inBattle = true;
            currentEnemy = tempEnemy;
            other.enabled = false;
        }
    }

    public void KillEnemy()
    {
        EnemyHealthBar.enemyHealthBarController.Deinitialization();

        currentEnemy.InvokeDeathAnimation();
        Invoke("EnemyHealthBar.enemyHealthBarController.ClosedHealthBar", 0.35f);
        inBattle = false;
    }
}