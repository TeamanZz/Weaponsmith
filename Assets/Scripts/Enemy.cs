using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int numberOfHitsToDeath = 3;

    public Transform healthPanel;

    public float receivingTime = 3f;

    public Slider healthBar;
    public Slider backHealthBar;
    [SerializeField] private float amountOfDamage;

    public int numberOfHits = 0;

    public BoxCollider triggerObject;
    public void Start()
    {
        amountOfDamage = 100 / numberOfHitsToDeath;
        numberOfHits = numberOfHitsToDeath;

        triggerObject.enabled = true;
    }

    [ContextMenu("Damage")]
    public void TakingDamage()
    {
        StartCoroutine(TakeDamage());
       
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
        triggerObject.enabled = false;

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
