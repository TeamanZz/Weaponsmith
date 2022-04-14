using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DungeonCharacter : MonoBehaviour
{
    public static DungeonCharacter Instance;

    [SerializeField] private float runSpeed;
    [SerializeField] private int criticalHitRate;
    [SerializeField] private int maxAllowedAttackAnimationsCount = 4;
    [SerializeField] private int boosterDamageCoefficient = 1;
    [SerializeField] private MeleeWeaponTrail weaponTrail;
    [SerializeField] private AudioSource audioSource;

    [Header("Health")]
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private Image healthImageFilled;

    [Space]
    [SerializeField] private List<AudioClip> sounds = new List<AudioClip>();

    [HideInInspector] public float lastSpeedUpValue;
    [HideInInspector] public int allowedAttackAnimationsCount = 2;
    [HideInInspector] public bool isInBattle = false;
    [HideInInspector] public bool canCriticalHit = false;
    [HideInInspector] private bool weaponTrailEnabled = false;
    [HideInInspector] public bool needSpeedUpOnAwake;
    [HideInInspector] public DungeonEnemy currentEnemy;
    [HideInInspector] public Animator animator;

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        allowedAttackAnimationsCount = PlayerPrefs.GetInt("allowedAttackAnimationsCount", 2);

        LoadWeaponTrailValue();
        LoadCriticalHitValue();

        if (needSpeedUpOnAwake)
            animator.speed = lastSpeedUpValue;
    }

    private void FixedUpdate()
    {
        if (!isInBattle)
            RunForward();
        else
            HandleBattle();
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
            if (weaponTrailEnabled)
                weaponTrail.Emit = true;
        }
    }

    public void TakeDamage(int damageValue)
    {
        currentHealth -= damageValue;
        UpdateHPBar();
    }

    private void UpdateHPBar()
    {
        var newBarValue = ((float)currentHealth / (float)maxHealth);
        healthImageFilled.DOFillAmount(newBarValue, 0.5f).SetEase(Ease.OutBack);
    }

    private void LoadWeaponTrailValue()
    {
        string trailEnabled = PlayerPrefs.GetString("weaponTrailEnabled", "false");
        if (trailEnabled == "true")
            weaponTrailEnabled = true;
        else
            weaponTrailEnabled = false;
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
        // PlayerPrefs.SetString("canCriticalHit", "true");
    }

    public void EnableWeaponTrail()
    {
        // PlayerPrefs.SetString("weaponTrailEnabled", "true");
        weaponTrailEnabled = true;
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
        // PlayerPrefs.SetInt("allowedAttackAnimationsCount", allowedAttackAnimationsCount);
        PlayerPrefs.Save();
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
        int isCrit = UnityEngine.Random.Range(0, criticalHitRate);
        if (isCrit == 0 && canCriticalHit)
            currentEnemy.enemyHealthBar.TakeDamageControll(damage: 1 * boosterDamageCoefficient, isDoubleDamage: true);
        else
            currentEnemy.enemyHealthBar.TakeDamageControll(damage: 1 * boosterDamageCoefficient);
    }

    public void KillEnemy()
    {
        currentEnemy.InvokeDeathAnimation();
        currentEnemy = null;
        animator.SetInteger("AttackIndex", -1);
        animator.SetBool("EnemyIsNear", false);
        animator.SetTrigger("EnemyDeath");
        isInBattle = false;
        if (weaponTrailEnabled)
            weaponTrail.Emit = false;
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