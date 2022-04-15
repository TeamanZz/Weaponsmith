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

    private IEnumerator IEAttack()
    {
        yield return new WaitForSeconds(Random.Range(4, 9));
        animator.SetTrigger("Attack");
        yield return IEAttack();
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

    public void HitPlayerCharacter()
    {
        DungeonCharacter.Instance.TakeDamage(1);
    }

    private void UpdateColliderPosition()
    {
        detectionCollider = GetComponent<BoxCollider>();
        detectionCollider.center = new Vector3(detectionCollider.center.x, detectionCollider.center.y, distanceToCollider);
    }

    public void PlayDamageAnimation()
    {
        PlayEnemyDamageSound();
        animator.Play("Recieve Damage", 1);
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