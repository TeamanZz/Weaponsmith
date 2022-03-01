using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DungeonEnemy : MonoBehaviour
{
    [SerializeField] private List<Animator> animators = new List<Animator>();
    public List<GameObject> currentEnemySkin = new List<GameObject>();
    public int skinCount;
    public GameObject blueprintPrefab;
    public float scale = 1.5f;
    public float distanceToCollider = 1.5f;
    public EnemyHealthBar enemyHealthBar;

    private BoxCollider detectionCollider;
    public float enemyLVL;
    public TextMeshProUGUI enemyLvlText;

    private int dropRate = 0;
    private int cuurentSkinIndex;

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
        enemyLvlText.text = enemyLVL.ToString() + " LVL";
    }

    private void InitializeVariables()
    {
        dropRate = PanelsHandler.Instance.dropChanceImprovements;
        skinCount = SkinsManager.Instance.dungeonEnemySkinCount;
    }

    private void SetScale()
    {
        transform.localScale = Vector3.one * scale;
    }

    private static void IncreaseBlueprintDropChance()
    {
        PanelsHandler.Instance.dropChanceImprovements -= 1;
        if (PanelsHandler.Instance.dropChanceImprovements <= 0)
            PanelsHandler.Instance.dropChanceImprovements = 7;
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
        if (skinCount > 0)
        {
            int random = Random.Range(0, SkinsManager.Instance.dungeonEnemySkinCount);
            random = Mathf.Clamp(random, 0, currentEnemySkin.Count);

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
        for (int i = 0; i < animators.Count; i++)
            animators[i].Play("Take Damage", 0, 0);
    }

    public void InvokeDeathAnimation()
    {
        if (ItemsManager.Instance.currentWaitingPanel != null && ItemsManager.Instance.currentWaitingPanel.currentState == PanelItemState.WaitingForDrawing)
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

        Destroy(gameObject, 10f);
    }
}