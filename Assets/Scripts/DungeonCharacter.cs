using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonCharacter : MonoBehaviour
{
    public static DungeonCharacter Instance;

    public float runSpeed;
    public bool isInBattle = false;
    public int allowedAttackAnimationsCount = 2;

    [HideInInspector] public DungeonEnemy currentEnemy;
    [HideInInspector] public Animator animator;

    public int maxAllowedAttackAnimationsCount = 4;

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        allowedAttackAnimationsCount = PlayerPrefs.GetInt("allowedAttackAnimationsCount", 2);
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

    public void IncreaseAllowedAttackAnimationsCount()
    {
        Debug.Log("PIZDEC 11");

        Debug.Log("Pizda" + allowedAttackAnimationsCount + "|" + maxAllowedAttackAnimationsCount);
        if (allowedAttackAnimationsCount >= maxAllowedAttackAnimationsCount)
            return;
        Debug.Log("PIZDEC 22");

        allowedAttackAnimationsCount++;
        PlayerPrefs.SetInt("allowedAttackAnimationsCount", allowedAttackAnimationsCount);
        PlayerPrefs.Save();

        Debug.Log("PIZDEC " + PlayerPrefs.GetInt("allowedAttackAnimationsCount"));
    }

    public void HandleBattle()
    {
        if (animator.GetBool("EnemyIsNear") == false)
        {
            animator.SetInteger("AttackIndex", UnityEngine.Random.Range(0, allowedAttackAnimationsCount));
            animator.SetBool("EnemyIsNear", true);
        }
    }

    public void HitEnemy()
    {
        animator.SetInteger("AttackIndex", UnityEngine.Random.Range(0, allowedAttackAnimationsCount));
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
            animator.SetInteger("AttackIndex", UnityEngine.Random.Range(0, allowedAttackAnimationsCount));
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