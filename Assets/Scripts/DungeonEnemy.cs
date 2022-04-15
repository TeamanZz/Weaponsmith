using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class DungeonEnemy : MonoBehaviour
{
    public EnemyHealthBar enemyHealthBar;

    [SerializeField] private GameObject chestPrefab;
    [SerializeField] private TextMeshProUGUI enemyLvlText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> sounds = new List<AudioClip>();
    [SerializeField] private float scale = 1.5f;
    [SerializeField] private float distanceToCollider = 1.5f;
    [SerializeField] private float enemyLVL;

    private int curentSkinIndex;
    private BoxCollider detectionCollider;
    public Animator animator;

    private bool isDead;
    private bool isInBattle;
    [HideInInspector] public DungeonCharacter currentEnemy;

    private void Start()
    {
        // InvokeRepeating("AttackCharacter", 2, 5);
    }

    private IEnumerator IEAttack()
    {
        yield return new WaitForSeconds(2);
        animator.SetInteger("AttackIndex", 1);
        // yield return new WaitForSeconds(2.667f);
        // animator.SetInteger("AttackIndex", 0);
        yield return IEAttack();
    }

    // private void FixedUpdate()
    // {
    //     if (isInBattle)
    //         HandleBattle();
    // }

    public void HandleBattle()
    {
        animator.SetInteger("AttackIndex", 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        DungeonCharacter dungeonCharacter;
        if (other.TryGetComponent<DungeonCharacter>(out dungeonCharacter))
        {
            isInBattle = true;
            currentEnemy = dungeonCharacter;
            other.enabled = false;
            StartCoroutine(IEAttack());
        }
    }

    private void OnDisable()
    {
        if (isDead)
            Destroy(gameObject);
    }

    public void Initialization()
    {
        SetScale();
        UpdateColliderPosition();
    }

    private void UpdateColliderPosition()
    {
        detectionCollider = GetComponent<BoxCollider>();
        detectionCollider.center = new Vector3(detectionCollider.center.x, detectionCollider.center.y, distanceToCollider);
    }

    public void PlayDamageAnimation()
    {
        PlayEnemyDamageSound();
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Enemy Attack")
            animator.SetTrigger("Damage");
    }

    public void InvokeDeathAnimation()
    {
        animator.SetTrigger("Death");
        isDead = true;
        PlayEnemyDeathSound();
        Destroy(gameObject, 10f);
    }

    private void SetEnemyLVL()
    {
        if (enemyLVL == 0)
            enemyLvlText.text = (enemyLVL + 1).ToString() + " LVL";
        else
            enemyLvlText.text = enemyLVL.ToString() + " LVL";
    }

    public void PlayEnemyDeathSound()
    {
        audioSource.PlayOneShot(sounds[Random.Range(3, 5)]);
    }

    public void PlayEnemyDamageSound()
    {
        audioSource.PlayOneShot(sounds[Random.Range(0, 3)]);
    }

    private void SetScale()
    {
        transform.localScale = Vector3.one * scale;
    }
}