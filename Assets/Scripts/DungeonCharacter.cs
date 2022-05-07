using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using TMPro;

public class DungeonCharacter : MonoBehaviour
{
    public static DungeonCharacter Instance;

    [SerializeField] private float runSpeed;
    [SerializeField] private int criticalHitRate;
    [SerializeField] private int maxAllowedAttackAnimationsCount = 4;
    [SerializeField] private int boosterDamageCoefficient = 1;
    [SerializeField] private MeleeWeaponTrail weaponTrail;
    [SerializeField] private AudioSource audioSource;

    [Header("Health And Stats")]
    [SerializeField] private float minAttackDelay = 2;
    [SerializeField] private float maxAttackDelay = 3.5f;

    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentArmor;
    [SerializeField] private int maxArmor;

    //[SerializeField] private int characterArmor;
    [SerializeField] private int characterDamage;

    [SerializeField] private Image healthImageFilled;
    [SerializeField] private Image armorImageFilled;

    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI armorText;

    [Space]
    [SerializeField] private List<AudioClip> sounds = new List<AudioClip>();

    [HideInInspector] public DungeonEnemy currentEnemy;
    [HideInInspector] public Animator animator;
    [HideInInspector] public float lastSpeedUpValue;
    [HideInInspector] public int allowedAttackAnimationsCount = 2;
    [HideInInspector] public bool isInBattle = false;
    [HideInInspector] public bool canCriticalHit = false;
    [HideInInspector] public bool needSpeedUpOnAwake;

    private CapsuleCollider detectionCollider;
    private Coroutine attackCoroutine;

    public DungeonPopupText damageTextPopup;

    public List<MeleeWeaponTrail> trailsList = new List<MeleeWeaponTrail>();

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        detectionCollider = GetComponent<CapsuleCollider>();
        allowedAttackAnimationsCount = PlayerPrefs.GetInt("allowedAttackAnimationsCount", 2);

        LoadCriticalHitValue();

        if (needSpeedUpOnAwake)
            animator.speed = lastSpeedUpValue;
    }

    public void EnableWeaponEmit()
    {
        for (int i = 0; i < trailsList.Count; i++)
        {
            if (trailsList[i].enabled)
                trailsList[i].Emit = true;
        }
    }

    public void DisableWeaponEmit()
    {
        for (int i = 0; i < trailsList.Count; i++)
        {
            if (trailsList[i].enabled)
                trailsList[i].Emit = false;
        }
    }

    public void HitEnemy()
    {
        var damageValue = characterDamage + Random.Range((-characterDamage / 5), (characterDamage / 5));

        if (damageValue == 0)
            damageValue = 1;

        currentEnemy.PlayDamageAnimation(damageValue);
        currentEnemy.enemyHealthBar.TakeDamage(damageValue);
    }

    public void InitializeStats(int hpValue, int armorValue, int damageValue)
    {
        currentArmor = armorValue;
        maxArmor = armorValue;

        characterDamage = damageValue;

        currentHealth = hpValue;
        maxHealth = hpValue;

        healthImageFilled.DOFillAmount(1, 0.5f).SetEase(Ease.OutBack);
        armorImageFilled.DOFillAmount(1, 0.5f).SetEase(Ease.OutBack);

        UpdateAllBars();
    }

    private IEnumerator IEAttack()
    {
        var attackSpeed = skillController.skills[4];
        float reloadTime;

        if (attackSpeed != null)
            reloadTime = attackSpeed.skillValue[attackSpeed.skillLvl];
        else
            reloadTime = 0;

        yield return new WaitForSeconds(Random.Range(minAttackDelay, maxAttackDelay - reloadTime));
        int attackAnimationIndex = Random.Range(1, 9);
        animator.SetTrigger("Attack" + attackAnimationIndex);
        // animator.SetTrigger("Attack" + 8);
        yield return IEAttack();
    }

    private void FixedUpdate()
    {
        if (!isInBattle)
            RunForward();
    }

    private void OnTriggerEnter(Collider other)
    {
        DungeonEnemy tempEnemy;
        if (other.TryGetComponent<DungeonEnemy>(out tempEnemy))
        {
            animator.SetBool("EnemyIsNear", true);
            isInBattle = true;
            currentEnemy = tempEnemy;
            detectionCollider.enabled = false;
            attackCoroutine = StartCoroutine(IEAttack());
        }
    }

    public int regenerationNumber = 2;
    public int maxNumber = 5;
    public SkillController skillController;
    public void TakeDamage(int damageValue)
    {
        var regenerationSkill = skillController.skills[3];

        int tempDamage;

        if (currentArmor > damageValue)
        {
            currentArmor -= damageValue;
            tempDamage = damageValue;
        }
        else
        {
            int checkDamage = damageValue - currentArmor;
            currentArmor = 0;

            currentHealth -= checkDamage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            int number = Random.Range(0, maxNumber);
            if (number == regenerationNumber)
            {
                float num = maxHealth / 100;
                int addHP = (int)(regenerationSkill.skillValue[regenerationSkill.skillLvl] * num);
                currentHealth += addHP;
                Debug.Log("Regeneration = " + addHP);
            }

            tempDamage = damageValue;
        }



        UpdateAllBars();

        hitParticles.Play();
        animator.Play("Recieve Damage", 1);
        var newPopup = Instantiate(damageTextPopup, transform.position + new Vector3(-2, 1, 0), Quaternion.Euler(32, 0, 0));
        newPopup.GetComponent<DungeonPopupText>().InitializeCharacterDamageText(tempDamage.ToString());

        if (currentHealth <= 0)
        {
            DungeonCharacter.Instance.DisableCharacterRun();
            DungeonRewardPanel.Instance.OpenRewardPanel(1);
            currentEnemy.gameObject.SetActive(false);
            detectionCollider.enabled = true;

        }
        PlayHitSound();
    }

    private void UpdateAllBars()
    {
        float newHpBarValue;
        if (maxHealth > 0)
            newHpBarValue = ((float)currentHealth / (float)maxHealth);
        else
            newHpBarValue = 0;

        healthImageFilled.DOFillAmount(newHpBarValue, 0.5f).SetEase(Ease.OutBack);
        hpText.text = currentHealth.ToString();

        float newArmorBarValue;
        if (maxArmor > 0)
            newArmorBarValue = ((float)currentArmor / (float)maxArmor);
        else
            newArmorBarValue = 0;

        armorImageFilled.DOFillAmount(newArmorBarValue, 0.5f).SetEase(Ease.OutBack);
        armorText.text = currentArmor.ToString();
    }

    private void LoadCriticalHitValue()
    {
        string criticalDamage = PlayerPrefs.GetString("canCriticalHit", "false");
        if (criticalDamage == "true")
            canCriticalHit = true;
        else
            canCriticalHit = false;
    }

    public void ChangeCharacterRunAndAttackSpeed(float newAnimatorCoefficient, bool needSpeedUp)
    {
        if (newAnimatorCoefficient != 1)
            runSpeed = 0.15f;
        else
            EnableCharacterRun();

        lastSpeedUpValue = newAnimatorCoefficient;

        if (animator != null)
        {
            animator.speed = newAnimatorCoefficient;
        }
        else
        {
            needSpeedUpOnAwake = needSpeedUp;
        }
    }

    public void ChangeCharacterDamageCoefficient(int value)
    {
        boosterDamageCoefficient = value;
    }

    public void EnableCriticalHit()
    {
        canCriticalHit = true;
    }

    private void RunForward()
    {
        transform.position += new Vector3(0, 0, runSpeed);
    }

    public void IncreaseAllowedAttackAnimationsCount()
    {
        if (allowedAttackAnimationsCount >= maxAllowedAttackAnimationsCount)
            return;

        allowedAttackAnimationsCount++;
        PlayerPrefs.Save();
    }

    public void KillEnemy()
    {
        currentEnemy.InvokeDeathAnimation();
        currentEnemy = null;
        animator.SetBool("EnemyIsNear", false);
        animator.SetTrigger("EnemyDeath");
        detectionCollider.enabled = true;
        isInBattle = false;

        StopCoroutine(attackCoroutine);
    }

    public void DisableCharacterRun()
    {
        runSpeed = 0;
    }

    public void EnableCharacterRun()
    {
        runSpeed = 0.1f;
    }

    public void PlayRoundHitSound()
    {
        audioSource.PlayOneShot(sounds[UnityEngine.Random.Range(0, 2)]);
    }

    public void PlayHitSound()
    {
        audioSource.PlayOneShot(sounds[2]);
    }
}