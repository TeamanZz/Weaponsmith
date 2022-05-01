using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class DungeonEnemy : MonoBehaviour
{
    public EnemyHealthBar enemyHealthBar;
    public Animator animator;
    public GameObject damageTextPopup;
    public int damage;

    [HideInInspector] public DungeonCharacter currentEnemy;

    [SerializeField] private GameObject chestPrefab;
    [SerializeField] private TextMeshProUGUI enemyLvlText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> sounds = new List<AudioClip>();
    [SerializeField] private float scale = 1.5f;
    [SerializeField] private float distanceToCollider = 1.5f;
    
    [SerializeField] private float enemyLVL;
    
    private BoxCollider detectionCollider;
    private bool isDead;
    private int curentSkinIndex;

    private IEnumerator IEAttack()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        animator.SetTrigger("Attack");
        yield return IEAttack();
    }

    private void OnTriggerEnter(Collider other)
    {
        DungeonCharacter dungeonCharacter;
        if (other.TryGetComponent<DungeonCharacter>(out dungeonCharacter))
        {
            currentEnemy = dungeonCharacter;
            detectionCollider.enabled = false;
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

        animator.SetFloat("Cycle Offset", Random.Range(0, 1f)); 
    }

    public void HitPlayerCharacter()
    {
        DungeonCharacter.Instance.TakeDamage(damage);
    }

    private void UpdateColliderPosition()
    {
        detectionCollider = GetComponent<BoxCollider>();
        detectionCollider.center = new Vector3(detectionCollider.center.x, detectionCollider.center.y, distanceToCollider);
    }

    public void PlayDamageAnimation(int value)
    {
        PlayEnemyDamageSound();
        animator.Play("Recieve Damage", 1);
        var newPopup = Instantiate(damageTextPopup, transform.position + new Vector3(2, 1, 0), Quaternion.Euler(32, 0, 0));
        newPopup.GetComponent<DungeonPopupText>().InitializeEnemyDamageText(value.ToString());
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