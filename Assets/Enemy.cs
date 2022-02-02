using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int numberOfHitsToDeath = 3;

    public float receivingTime = 3f;

    public Slider healthBar;
    public Slider backHealthBar;
    [SerializeField] private float amountOfDamage;

    public int numberOfHits = 0;
    public void Awake()
    {
        amountOfDamage = 100 / numberOfHitsToDeath;
        numberOfHits = numberOfHitsToDeath;
    }

    [ContextMenu("Damage")]
    public void TakingDamage()
    {
        float currentValue;

        //if (healthBar.value - amountOfDamage < 10)
        //{
        //    for (int i = 0; i < healthBar.value * Time.deltaTime; i++)
        //    {
        //        healthBar.value -= 1;
        //    }
        //    //currentValue = 0;
        //}
        //else
        //{
        //    for (int i = 0; i < amountOfDamage * Time.deltaTime; i++)
        //    {
        //        healthBar.value -= 1;
        //    }
        //}
            //currentValue = healthBar.value - amountOfDamage;

        //healthBar.value = Mathf.MoveTowards(healthBar.value, currentValue, receivingTime);

        StartCoroutine(TakeDamage());
       
        //for(int i = 0; i < amountOfDamage; i ++)
        //{
        //    healthBar.value -= 1;
        //}
        Debug.Log(healthBar.value);
    }

    public IEnumerator TakeDamage()
    {
        if (healthBar.value - amountOfDamage < amountOfDamage)
        {
            float value = healthBar.value;
            for (int i = 0; i < value; i++)
            {
                healthBar.value -= 1;
                yield return new WaitForSeconds(0.02f);
            }

            for (int i = 0; i < value; i++)
            {
                backHealthBar.value -= 1;
                yield return new WaitForSeconds(0.02f);
            }

            KillingEnemy();
        }
        else
        {
            for (int i = 0; i < amountOfDamage; i++)
            {
                healthBar.value -= 1;
                yield return new WaitForSeconds(0.02f);
            }

            for (int i = 0; i < amountOfDamage; i++)
            {
                backHealthBar.value -= 1;
                yield return new WaitForSeconds(0.02f);
            }
        }
        
    }
    public void KillingEnemy()
    {
        Character.character.enemies.Remove(this);
        Character.character.inAttack = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            Character.character.enemies.Add(this);
        }
    }
}
