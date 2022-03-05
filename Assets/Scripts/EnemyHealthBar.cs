using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("Enemy health bar settings")]
    public GameObject canvas;
    public GameObject healthBar;
    public Image healthBarImage;

    private DungeonEnemy enemyComponent;

    public int maxHealth = 5;
    public int currentHealth = 5;
    public ParticleSystem hitParticles;
    public ParticleSystem doubleHitParticles;
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
    public void TakeDamageControll(int damage = 1, bool isDoubleDamage = false)
    {
        if (enemyComponent == null)
        {
            DungeonCharacter.Instance.isInBattle = false;
            // DungeonCharacter.Instance.animator.SetTrigger("EnemyIsNear");
            return;
        }

        if (isDoubleDamage)
        {
            currentHealth -= damage * 2;
            hitParticles.Play();
            doubleHitParticles.Play();
            healthBar.transform.DOShakePosition(0.5f, randomness: 20, strength: 0.2f);
        }
        else
        {
            currentHealth -= damage;
            hitParticles.Play();
        }

        var newBarValue = ((float)currentHealth / (float)maxHealth);
        healthBarImage.DOFillAmount(newBarValue, 0.5f).SetEase(Ease.OutBack);

        if (currentHealth <= 0)
        {
            DungeonCharacter.Instance.KillEnemy();
            canvas.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
            deathParticles.Play();
            return;
        }
    }
}