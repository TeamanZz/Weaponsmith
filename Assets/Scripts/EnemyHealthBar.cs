using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("Enemy health bar settings")]
    public GameObject enemyHealthBar;
    public Image healthBarImage;

    private DungeonEnemy enemyComponent;

    public int maxHealth = 5;
    public int currentHealth = 5;
    public ParticleSystem hitParticles;
    public ParticleSystem deathParticles;
    public void Awake()
    {
        enemyComponent = GetComponent<DungeonEnemy>();
    }

    public void InitializeHP(int value)
    {
        maxHealth = value;
        currentHealth = value;
    }

    [ContextMenu("Take damage")]
    public void TakeDamageControll(float damage = 1)
    {
        if (enemyComponent == null)
        {
            DungeonCharacter.Instance.isInBattle = false;
            // DungeonCharacter.Instance.animator.SetTrigger("EnemyIsNear");
            return;
        }

        Debug.Log("Damage");

        currentHealth -= 1;
        var newBarValue = ((float)currentHealth / (float)maxHealth);
        healthBarImage.DOFillAmount(newBarValue, 0.5f).SetEase(Ease.OutBack);

        hitParticles.Play();
        if (currentHealth <= 0)
        {
            DungeonCharacter.Instance.KillEnemy();
            enemyHealthBar.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
            deathParticles.Play();
            return;
        }
        Debug.Log(currentHealth);
    }
}