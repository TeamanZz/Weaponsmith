using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonEnemy : MonoBehaviour
{
    public EnemyHealthBar enemyHealthBar;

    [SerializeField] private GameObject chestPrefab;
    [SerializeField] private TextMeshProUGUI enemyLvlText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<GameObject> currentEnemySkin = new List<GameObject>();
    [SerializeField] private List<AudioClip> sounds = new List<AudioClip>();
    [SerializeField] private float scale = 1.5f;
    [SerializeField] private float distanceToCollider = 1.5f;
    [SerializeField] private float enemyLVL;

    private int curentSkinIndex;
    private BoxCollider detectionCollider;
    private List<Animator> animators = new List<Animator>();

    private bool isDead;
    private bool isInBattle;
    [HideInInspector] public DungeonCharacter currentEnemy;

    private void Start()
    {
        // InvokeRepeating("AttackCharacter", 2, 5);
    }

    private void FixedUpdate()
    {
        if (isInBattle)
            HandleBattle();
    }

    public void HandleBattle()
    {
        for (int i = 0; i < animators.Count; i++)
            animators[i].SetInteger("AttackIndex", 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        DungeonCharacter dungeonCharacter;
        if (other.TryGetComponent<DungeonCharacter>(out dungeonCharacter))
        {
            isInBattle = true;
            currentEnemy = dungeonCharacter;
            other.enabled = false;
            for (int i = 0; i < animators.Count; i++)
                animators[i].SetInteger("AttackIndex", 1);
        }
    }

    private void OnDisable()
    {
        if (isDead)
            Destroy(gameObject);
    }

    public void Initialization()
    {
        AddAnimatorToAnimatorsList();
        SetEnemySkin();
        SetScale();
        UpdateColliderPosition();
    }

    public void AttackCharacter()
    {
        for (int i = 0; i < animators.Count; i++)
            animators[i].Play("Attack", 0, 0);
    }

    private void AddAnimatorToAnimatorsList()
    {
        animators.Add(GetComponent<Animator>());
        animators.AddRange(GetComponentsInChildren<Animator>());
    }

    private void UpdateColliderPosition()
    {
        detectionCollider = GetComponent<BoxCollider>();
        detectionCollider.center = new Vector3(detectionCollider.center.x, detectionCollider.center.y, distanceToCollider);
    }

    private void SetEnemySkin()
    {
        if (SkinsManager.Instance.dungeonEnemySkinCount > 0)
        {
            int random = Random.Range(0, SkinsManager.Instance.dungeonEnemySkinCount + 1);

            foreach (GameObject obj in currentEnemySkin)
            {
                obj.SetActive(false);
            }

            currentEnemySkin[random].SetActive(true);
            curentSkinIndex = random;
        }
        else
        {
            currentEnemySkin[0].SetActive(true);
        }
    }

    public void PlayDamageAnimation()
    {
        PlayEnemyDamageSound();

        for (int i = 0; i < animators.Count; i++)
            animators[i].SetTrigger("Damage");
    }

    public void InvokeDeathAnimation()
    {
        for (int i = 0; i < animators.Count; i++)
            animators[i].SetTrigger("Death");
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