using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonCharacter : MonoBehaviour
{
    public static DungeonCharacter Instance;

    public float runSpeed;

    [Header("Battle settings")]
    public bool isInBattle = false;
    public DungeonEnemy currentEnemy;

    [HideInInspector] public Animator animator;

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isInBattle)
            RunForward();
        else
            HandleBattle();
    }

    private void RunForward()
    {
        transform.position += new Vector3(0, 0, runSpeed);
    }

    public void HandleBattle()
    {
        if (animator.GetBool("EnemyIsNear") == false)
        {
            animator.SetInteger("AttackIndex", UnityEngine.Random.Range(0, 4));
            animator.SetBool("EnemyIsNear", true);
        }
    }

    public void HitEnemy()
    {
        animator.SetInteger("AttackIndex", UnityEngine.Random.Range(0, 4));
        currentEnemy.PlayDamageAnimation();
        currentEnemy.enemyHealthBar.TakeDamageControll();
    }

    private void OnTriggerEnter(Collider other)
    {
        DungeonEnemy tempEnemy;
        if (other.TryGetComponent<DungeonEnemy>(out tempEnemy))
        {
            isInBattle = true;
            currentEnemy = tempEnemy;
            other.enabled = false;
            animator.SetInteger("AttackIndex", UnityEngine.Random.Range(0, 4));
        }
    }

    public void KillEnemy()
    {
        currentEnemy.InvokeDeathAnimation();
        currentEnemy = null;
        animator.SetInteger("AttackIndex", -1);
        animator.SetBool("EnemyIsNear", false);
        animator.SetTrigger("EnemyDeath");
        isInBattle = false;
    }
}