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
    public Coroutine currentCorrutine;

    public GameObject enemyHealthBar;
    public Slider healthBar;
    public Slider backHealthBar;
    [SerializeField] private float amountOfDamage;

    public bool isInitialization = false;
    public DungeonEnemy targetEnemyController;

    public bool endCoroutine = false;
    public void Awake()
    {
        enemyHealthBarController = this;
        healthBar.maxValue = 100;
        backHealthBar.maxValue = 100;

        enemyHealthBar.SetActive(false);
    }
    public void Initialization(DungeonEnemy enemy)
    {
        dungeonCharacter = DungeonCharacter.dungeonCharacter;
        targetEnemyController = enemy;
        amountOfDamage = 100 / enemy.numberOfHits;

        Debug.Log("Amount of damage " + amountOfDamage);

        healthBar.value = 100;
        backHealthBar.value = 100;

        isInitialization = true;
    }

    public void Deinitialization()
    {
        targetEnemyController = null;
        isInitialization = false;
        ClosedHealthBar();
    }

    public void OpenHealthBar()
    {
        isOpen = true;
        if(currentCorrutine != null)
             currentCorrutine = StartCoroutine(TakeDamage());

        enemyHealthBar.SetActive(true);
    }

    public void ClosedHealthBar()
    {
        isOpen = false;
        if(currentCorrutine != null)
           StopCoroutine(currentCorrutine);
        
        enemyHealthBar.SetActive(false);
    }

    [ContextMenu("Take damage")]
    public void TakeDamageControll()
    {
        if (endCoroutine == true)
            return;

        endCoroutine = true;
        Debug.Log("Take damage");
        if(currentCorrutine == null)
            currentCorrutine = StartCoroutine(TakeDamage());
        return;
    }
    public IEnumerator TakeDamage()
    {
        Debug.Log("Take \nOst value = " + healthBar.value);
        if(healthBar.value <= 0)
        {
            dungeonCharacter.KillEnemy();
            Deinitialization();
            ClosedHealthBar();
            yield break;
        }

        float time = 0.025f;
        if (healthBar.value - amountOfDamage < amountOfDamage)
        {
            float value = healthBar.value;
            if (targetEnemyController != null)
                targetEnemyController.numberOfHits -= 1;

            for (int i = 0; i < value; i++)
            {
                healthBar.value -= 1;
                yield return new WaitForSeconds(time);
            }

            for (int i = 0; i < value; i++)
            {
                backHealthBar.value -= 1;
                yield return new WaitForSeconds(time);
            }

            if (dungeonCharacter.currentEnemy != null) { 

            endCoroutine = false; dungeonCharacter.KillEnemy();
        }
            currentCorrutine = null;
        }
        else
        {
            for (int i = 0; i < amountOfDamage; i++)
            {
                healthBar.value -= 1;
                yield return new WaitForSeconds(time);
            }

            for (int i = 0; i < amountOfDamage; i++)
            {
                backHealthBar.value -= 1;
                yield return new WaitForSeconds(time);
            }
        }

        endCoroutine = false;
    }

}
