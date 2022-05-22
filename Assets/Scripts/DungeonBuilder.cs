using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DungeonBuilder : MonoBehaviour
{
    public static DungeonBuilder Instance;

    [Header("Dungeon Pieces Spawn")]
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private Transform piecesContainer;
    [SerializeField] private int startDungeonPiecesCount = 40;
    [SerializeField] private Transform enemiesContainer;
    [SerializeField] private GameObject chestPrefab;
    [SerializeField] private GameObject endWallPrefab;

    [Header("Enemy Spawn")]
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();

    [Header("Entering Panel")]
    public TextMeshProUGUI levelName;
    public Image levelLogo;

    [Space]
    public List<DungeonLevelSettings> levels = new List<DungeonLevelSettings>();

    private GameObject lastSpawnedPiece;
    public DungeonLevelSettings currentLevelSettings;
    private int lastSpawnedPieceZPos;
    private int lastSpawnedEnemyZPos;
    private List<GameObject> enemiesList = new List<GameObject>();
    private List<GameObject> buildedPiecesList = new List<GameObject>();

    public DungeonChest lastChest;
    public SkillController skillController;

    public DungeonRewardPanel rewardPanel;
    public DungeonManager dungeonManager;

    public bool endWallWasSpawned;

    private void Awake()
    {
        Instance = this;
    }

    private void ResetVariablesValues()
    {
        lastSpawnedPieceZPos = 0;
        lastSpawnedEnemyZPos = 0;
        endWallWasSpawned = false;
    }

    public void BuildDungeon()
    {
        lastChest = null;
        currentLevelSettings = levels[dungeonManager.currentDungeonLevelId];
        levelLogo.sprite = currentLevelSettings.levelLogo;
        levelName.text = currentLevelSettings.levelName;

        int value = skillController.improvementCount + skillController.improvementCount;
        if (value >= skillController.maxPoints)
        {
            rewardPanel.crystalGroup.SetActive(false);
            Debug.Log("Crystal Closed");
        }
        else
        {
            rewardPanel.crystalGroup.SetActive(true);
            Debug.Log("Crystal Open");
        }

        var nextItem = CraftPanelItemsManager.Instance.craftPanelItemsList.Find(x => x.currentState == PanelItemState.Unknown);
        if (nextItem != null)
        {
            rewardPanel.blueprintGroup.SetActive(true);
            Debug.Log("Blueprint Closed");
        }
        else
        {
            rewardPanel.blueprintGroup.SetActive(false);
            Debug.Log("Blueprint Open");
        }

        Debug.Log("Value =" + value + "Max Point =" + SkillController.skillController.maxPoints);

        ResetVariablesValues();
        SpawnFirstWaveEnemies();
        SpawnChestWithGold();
        SpawnSecondWaveEnemies();
        SpawnBoss();
        SpawnChestWithBlueprint();
        SpawnDungeonPieces();
    }

    public void ResetDungeon()
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            Destroy(enemiesList[i]);
        }

        for (int i = 0; i < buildedPiecesList.Count; i++)
        {
            Destroy(buildedPiecesList[i]);
        }

        enemiesList.Clear();
        buildedPiecesList.Clear();
    }

    private void SpawnChestWithGold()
    {
        int newZPos = Random.Range(currentLevelSettings.minDistanceBetweenEnemies, currentLevelSettings.maxDistanceBetweenEnemies) + lastSpawnedEnemyZPos;

        GameObject chestObject = Instantiate(chestPrefab, Vector3.zero, Quaternion.identity, piecesContainer);
        DungeonChest chestComponent = chestObject.GetComponent<DungeonChest>();
        chestComponent.Initialize(DungeonChest.ChestFilling.Money, currentLevelSettings.firstChestGold);
        chestObject.transform.localPosition = new Vector3(0, 0, newZPos);
        lastSpawnedEnemyZPos = newZPos;
        buildedPiecesList.Add(chestObject);
    }

    private void SpawnChestWithBlueprint()
    {
        int newZPos = Random.Range(currentLevelSettings.minDistanceBetweenEnemies, currentLevelSettings.maxDistanceBetweenEnemies) + lastSpawnedEnemyZPos;

        GameObject chestObject = Instantiate(chestPrefab, Vector3.zero, Quaternion.identity, piecesContainer);
        DungeonChest chestComponent = chestObject.GetComponent<DungeonChest>();
        chestComponent.Initialize(DungeonChest.ChestFilling.BlueprintAndMoney, currentLevelSettings.secondChestGold);
        rewardPanel.InitializeBlueprintItem();

        lastChest = chestComponent;

        chestObject.transform.localPosition = new Vector3(0, 0, newZPos);
        lastSpawnedEnemyZPos = newZPos;
        buildedPiecesList.Add(chestObject);
    }

    public void SpawnPiece()
    {
        if (lastSpawnedPieceZPos > lastSpawnedEnemyZPos)
        {
            if (!endWallWasSpawned)
            {
                lastSpawnedPiece = Instantiate(endWallPrefab, new Vector3(0, 0, lastSpawnedPieceZPos), Quaternion.identity, piecesContainer);
                lastSpawnedPiece.transform.localPosition = new Vector3(0, 0, lastSpawnedPiece.transform.position.z);
                buildedPiecesList.Add(lastSpawnedPiece);
                endWallWasSpawned = true;
            }
        }
        else
        {
            lastSpawnedPiece = Instantiate(piecePrefab, new Vector3(0, 0, lastSpawnedPieceZPos), Quaternion.identity, piecesContainer);
            lastSpawnedPiece.transform.localPosition = new Vector3(0, 0, lastSpawnedPiece.transform.position.z);
            lastSpawnedPieceZPos += 5;
            buildedPiecesList.Add(lastSpawnedPiece);
        }
    }

    public void SpawnEnemy(int hpValue, int damageValue, float modelScale, float distanceCoefficient = 1, bool isBoss = false)
    {
        int newZPos = (int)((Random.Range(currentLevelSettings.minDistanceBetweenEnemies, currentLevelSettings.maxDistanceBetweenEnemies) + lastSpawnedEnemyZPos) * distanceCoefficient);
        var newEnemyRotation = Quaternion.Euler(0, 180, 0);

        GameObject enemy;
        if (isBoss)
            enemy = Instantiate(currentLevelSettings.levelBoss, Vector3.zero, newEnemyRotation, enemiesContainer);
        else
            enemy = Instantiate(enemyPrefabs[Random.Range(0, currentLevelSettings.maxAllowedEnemySkinIndex + 1)], Vector3.zero, newEnemyRotation, enemiesContainer);

        var enemyComponent = enemy.GetComponent<DungeonEnemy>();
        enemyComponent.Initialization();

        if (isBoss)
        {
            var bossComponent = enemy.GetComponent<DungeonBoss>();
            bossComponent.Initialize(currentLevelSettings.bossName);
        }

        var enemyHealthComponent = enemy.GetComponent<EnemyHealthBar>();
        enemyHealthComponent.InitializeHP(hpValue);

        enemyComponent.damage = damageValue;

        enemy.transform.localPosition = new Vector3(0, 0, newZPos);
        enemy.transform.localScale = Vector3.one * modelScale;

        lastSpawnedEnemyZPos = newZPos;
        enemiesList.Add(enemy);
    }

    private void SpawnBoss()
    {
        SpawnEnemy(currentLevelSettings.bossHealth, currentLevelSettings.bossDamage, currentLevelSettings.bossScale, 1.5f, true);
        DungeonManager.Instance.bossZPosition = lastSpawnedEnemyZPos;
    }

    private void SpawnSecondWaveEnemies()
    {
        for (int i = 0; i < currentLevelSettings.secondWaveEnemiesCount; i++)
        {
            SpawnEnemy(Random.Range(currentLevelSettings.minEnemiesHealth, currentLevelSettings.maxEnemiesHealth), Random.Range(currentLevelSettings.minEnemiesDamage, currentLevelSettings.maxEnemiesDamage), 1.5f);
        }
    }

    private void SpawnFirstWaveEnemies()
    {
        for (int i = 0; i < currentLevelSettings.firstWaveEnemiesCount; i++)
        {
            SpawnEnemy(Random.Range(currentLevelSettings.minEnemiesHealth, currentLevelSettings.maxEnemiesHealth), Random.Range(currentLevelSettings.minEnemiesDamage, currentLevelSettings.maxEnemiesDamage), 1.5f);
        }
    }

    private void SpawnDungeonPieces()
    {
        for (int i = 0; i <= startDungeonPiecesCount; i++)
        {
            SpawnPiece();
        }
    }

    [System.Serializable]
    public class DungeonLevelSettings
    {
        public int index;

        [Header("Gold Settings")]
        public int minGoldPerEnemy;
        public int maxGoldPerEnemy;
        public int firstChestGold;
        public int secondChestGold;

        [Header("Entering Panel")]
        public string levelName;
        public Sprite levelLogo;

        [Header("Enemies Settings")]
        public int minEnemiesHealth;
        public int maxEnemiesHealth;
        public int minEnemiesDamage;
        public int maxEnemiesDamage;
        public int maxAllowedEnemySkinIndex;
        public int firstWaveEnemiesCount;
        public int secondWaveEnemiesCount;
        public int minDistanceBetweenEnemies = 5;
        public int maxDistanceBetweenEnemies = 10;

        [Header("Boss Settings")]
        public GameObject levelBoss;
        public string bossName;
        public int bossHealth = 12;
        public float bossScale = 3f;
        public int bossDamage;
    }
}