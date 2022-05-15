using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class EnemyHealthBar : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth = 5;

    [Header("Enemy health bar settings")]
    public GameObject canvas;
    public GameObject healthBar;
    public Image healthBarImage;

    [Header("Particles")]
    public ParticleSystem hitParticles;
    public ParticleSystem doubleHitParticles;
    public ParticleSystem deathParticles;
    public ParticleSystem goldParticles;

    private DungeonEnemy enemyComponent;

    public TextMeshProUGUI enemyHPText;

    public void Awake()
    {
        enemyComponent = GetComponent<DungeonEnemy>();
    }

    public void InitializeHP(int value)
    {
        maxHealth = value;
        currentHealth = value;
        enemyHPText.text = currentHealth.ToString();
    }

    [ContextMenu("Take damage")]
    public void TakeDamage(int damage = 1)//, bool isDoubleDamage = false)
    {
        bool isDoubleDamage = false;
        if (enemyComponent == null)
        {
            DungeonCharacter.Instance.isInBattle = false;
            return;
        }

        var hitSkill = SkillController.skillController.mainSkillsData[5];
        int maxNumber = 0;
    
        if(hitSkill != null)
           maxNumber = (int)hitSkill.skillValue[hitSkill.skillLvl];

        int number = Random.Range(0, maxNumber);
        if (number == 2)
        {
            isDoubleDamage = true;
            Debug.Log("Damage " + isDoubleDamage);
        }
        else
            isDoubleDamage = false;

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
        
        enemyHPText.text = currentHealth.ToString();

        if (currentHealth <= 0)
        {
            //  + money     currentLevelEarnedMoneyCount
            int money = Random.Range(DungeonBuilder.Instance.levels[DungeonManager.Instance.currentDungeonLevelId].minGoldPerEnemy, DungeonBuilder.Instance.levels[DungeonManager.Instance.currentDungeonLevelId].maxGoldPerEnemy);
            MoneyHandler.Instance.moneyCount += money;
            DungeonManager.Instance.currentLevelEarnedMoneyCount += money;

            DungeonCharacter.Instance.KillEnemy();
            canvas.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
            deathParticles.Play();
            goldParticles.Play();
            return;
        }
    }
}