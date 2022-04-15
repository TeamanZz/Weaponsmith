using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

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
    [SerializeField] private ParticleSystem hitParticles;

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

    private IEnumerator IEAttack()
    {
        yield return new WaitForSeconds(Random.Range(4, 7));
        animator.SetTrigger("Attack");
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
            other.enabled = false;
            StartCoroutine(IEAttack());
            if (weaponTrailEnabled)
                weaponTrail.Emit = true;
        }
    }

    public void TakeDamage(int damageValue)
    {
        currentHealth -= damageValue;
        UpdateHPBar();
        hitParticles.Play();
        animator.Play("Recieve Damage", 1);
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

    public void HitEnemy()
    {
        // animator.SetInteger("AttackIndex", UnityEngine.Random.Range(0, allowedAttackAnimationsCount));
        currentEnemy.PlayDamageAnimation();
        currentEnemy.enemyHealthBar.TakeDamage(damage: 1);
        // int isCrit = UnityEngine.Random.Range(0, criticalHitRate);
        // if (isCrit == 0 && canCriticalHit)
        //     currentEnemy.enemyHealthBar.TakeDamageControll(damage: 1 * boosterDamageCoefficient, isDoubleDamage: true);
        // else
        //     currentEnemy.enemyHealthBar.TakeDamageControll(damage: 1 * boosterDamageCoefficient);
    }

    public void KillEnemy()
    {
        currentEnemy.InvokeDeathAnimation();
        currentEnemy = null;
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