using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonEnemy : MonoBehaviour
{
    public float scale = 1.5f;
    public float distanceToCollider = 1.5f;
    public float enemyLVL;
    public GameObject blueprintPrefab;
    public EnemyHealthBar enemyHealthBar;
    public TextMeshProUGUI enemyLvlText;
    public AudioSource audioSource;
    public List<GameObject> currentEnemySkin = new List<GameObject>();
    public List<AudioClip> sounds = new List<AudioClip>();

    private int dropRate = 0;
    private int cuurentSkinIndex;
    private BoxCollider detectionCollider;
    private List<Animator> animators = new List<Animator>();

    public void PlayEnemyDeathSound()
    {
        audioSource.PlayOneShot(sounds[Random.Range(3, 5)]);
    }

    public void PlayEnemyDamageSound()
    {
        audioSource.PlayOneShot(sounds[Random.Range(0, 3)]);
    }

    private void Start()
    {
        AddAnimatorToAnimatorsList();
        InitializeVariables();
        SetEnemySkin();
        IncreaseBlueprintDropChance();
        SetScale();
        HandleDistanceCollider();
        SetEnemyLVL();
    }

    private void SetEnemyLVL()
    {
        DungeonPanelItem enemyStats = DungeonBuilder.Instance.enemiesSkinsStats[cuurentSkinIndex].GetComponent<DungeonPanelItem>();
        enemyLVL = enemyStats.buysCount;

        if (enemyLVL == 0)
            enemyLvlText.text = (enemyLVL + 1).ToString() + " LVL";
        else
            enemyLvlText.text = enemyLVL.ToString() + " LVL";
    }

    private void InitializeVariables()
    {
        dropRate = PanelsHandler.Instance.dropChanceImprovements;
    }

    private void SetScale()
    {
        transform.localScale = Vector3.one * scale;
    }

    private static void IncreaseBlueprintDropChance()
    {
        PanelsHandler.Instance.dropChanceImprovements -= 1;
        if (PanelsHandler.Instance.dropChanceImprovements <= 0)
            PanelsHandler.Instance.dropChanceImprovements = 12;
    }

    private void AddAnimatorToAnimatorsList()
    {
        animators.Add(GetComponent<Animator>());
        animators.AddRange(GetComponentsInChildren<Animator>());
    }

    private void HandleDistanceCollider()
    {
        detectionCollider = GetComponent<BoxCollider>();
        detectionCollider.center = new Vector3(detectionCollider.center.x, detectionCollider.center.y, distanceToCollider);
    }

    private void SetEnemySkin()
    {
        if (SkinsManager.Instance.dungeonEnemySkinCount > 0)
        {
            int random = Random.Range(0, SkinsManager.Instance.dungeonEnemySkinCount + 1);

            Debug.Log("Enemy skin index: " + random);

            foreach (GameObject obj in currentEnemySkin)
            {
                obj.SetActive(false);
            }
            currentEnemySkin[random].SetActive(true);
            cuurentSkinIndex = random;
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
            animators[i].Play("Take Damage", 0, 0);
    }

    public void InvokeDeathAnimation()
    {
        if (CraftPanelItemsManager.Instance.currentWaitingPanel != null && CraftPanelItemsManager.Instance.currentWaitingPanel.currentState == PanelItemState.WaitingForDrawing)
        {
            int val = Random.Range(0, dropRate);
            Debug.Log("Value - " + val + " | " + "Drop rate - " + dropRate);

            if (val == 0)
            {
                Instantiate(blueprintPrefab, transform.position + new Vector3(0, 0.325f, 0), Quaternion.identity, transform.parent);
                PanelsHandler.Instance.dropChanceImprovements = 7;
            }
        }

        for (int i = 0; i < animators.Count; i++)
            animators[i].SetTrigger("Death");
        PlayEnemyDeathSound();
        Destroy(gameObject, 10f);
    }
}