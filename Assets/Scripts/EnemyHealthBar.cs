using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public static EnemyHealthBar enemyHealthBarController;
    public DungeonCharacter dungeonCharacter;

    [Header("Enemy health bar settings")]
    public bool isOpen = true;

    public GameObject enemyHealthBar;
    public Slider healthBar;
    [SerializeField] private float amountOfDamage;

    public bool isInitialization = false;
    public DungeonEnemy targetEnemy;

    public void Awake()
    {
        enemyHealthBarController = this;
        healthBar.maxValue = 100;

        enemyHealthBar.SetActive(false);
    }
    public void Initialization(DungeonEnemy enemy)
    {
        dungeonCharacter = DungeonCharacter.dungeonCharacter;
        targetEnemy = enemy;

        healthBar.value = 100;
        amountOfDamage = 100 / enemy.numberOfHits;

        Debug.Log("Amount of damage " + amountOfDamage);


        isInitialization = true;
    }

    public void OpenHealthBar()
    {
        enemyHealthBar.SetActive(true);
        isOpen = true;
    }

    [ContextMenu("Take damage")]
    public void TakeDamageControll()
    {
        if (targetEnemy == null)
        {
            dungeonCharacter.inBattle = false;
            dungeonCharacter.animator.SetTrigger("EnemyIsNear");
            return;
        }

        Debug.Log("Damage");
        
        targetEnemy.numberOfHits -= 1;

        if (targetEnemy.numberOfHits <= 0)
        {
            dungeonCharacter.KillEnemy();
            return;
        }

        healthBar.value -= amountOfDamage;

        Debug.Log(targetEnemy.numberOfHits);
    }

    public void Deinitialization()
    {
        targetEnemy = null;
        isInitialization = false;
        ClosedHealthBar();
    }

    public void ClosedHealthBar()
    {
        enemyHealthBar.SetActive(false);
        isOpen = false;
    }


}
