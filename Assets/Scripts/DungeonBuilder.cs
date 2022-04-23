using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBuilder : MonoBehaviour
{
    public static DungeonBuilder Instance;

    [Header("Dungeon Pieces Spawn")]
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private Transform piecesContainer;

    [SerializeField] private int startDungeonPiecesCount = 40;

    [Header("Enemy Spawn")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemiesContainer;

    [SerializeField] private int firstWaveEnemiesCount;
    [SerializeField] private int secondWaveEnemiesCount;

    [SerializeField] private int minDistanceBetweenEnemies = 5;
    [SerializeField] private int maxDistanceBetweenEnemies = 10;

    [Header("Boss Settings")]
    [SerializeField] private int bossHealth = 12;
    [SerializeField] private float bossScale = 3f;
    [SerializeField] private int bossDamage;

    [SerializeField] private int minEnemiesHealth;
    [SerializeField] private int maxEnemiesHealth;

    [SerializeField] private int minEnemiesDamage;
    [SerializeField] private int maxEnemiesDamage;


    [SerializeField] private GameObject chestPrefab;

    private GameObject lastSpawnedPiece;
    private int lastSpawnedPieceZPos;
    private int lastSpawnedEnemyZPos;

    private List<GameObject> enemiesList = new List<GameObject>();
    private List<GameObject> buildedPiecesList = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void ResetVariablesValues()
    {
        lastSpawnedPieceZPos = 0;
        lastSpawnedEnemyZPos = 0;
    }

    public void BuildDungeon()
    {
        ResetVariablesValues();
        SpawnDungeonPieces();
        SpawnFirstWaveEnemies();
        SpawnChestWithGold();
        SpawnSecondWaveEnemies();
        SpawnBoss();
        SpawnChestWithBlueprint();
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
        int newZPos = Random.Range(minDistanceBetweenEnemies, maxDistanceBetweenEnemies) + lastSpawnedEnemyZPos;

        GameObject chestObject = Instantiate(chestPrefab, Vector3.zero, Quaternion.identity, piecesContainer);
        DungeonChest chestComponent = chestObject.GetComponent<DungeonChest>();
        chestComponent.Initialize(DungeonChest.ChestFilling.Money);
        chestObject.transform.localPosition = new Vector3(0, 0, newZPos);
        lastSpawnedEnemyZPos = newZPos;
        buildedPiecesList.Add(chestObject);
    }

    private void SpawnChestWithBlueprint()
    {
        int newZPos = Random.Range(minDistanceBetweenEnemies, maxDistanceBetweenEnemies) + lastSpawnedEnemyZPos;

        GameObject chestObject = Instantiate(chestPrefab, Vector3.zero, Quaternion.identity, piecesContainer);
        DungeonChest chestComponent = chestObject.GetComponent<DungeonChest>();
        chestComponent.Initialize(DungeonChest.ChestFilling.BlueprintAndMoney);
        DungeonRewardPanel.Instance.InitializeBlueprintItem();

        chestObject.transform.localPosition = new Vector3(0, 0, newZPos);
        lastSpawnedEnemyZPos = newZPos;
        buildedPiecesList.Add(chestObject);
    }

    public void SpawnPiece()
    {
        lastSpawnedPiece = Instantiate(piecePrefab, new Vector3(0, 0, lastSpawnedPieceZPos), Quaternion.identity, piecesContainer);
        lastSpawnedPiece.transform.localPosition = new Vector3(0, 0, lastSpawnedPiece.transform.position.z);
        lastSpawnedPieceZPos += 5;
        buildedPiecesList.Add(lastSpawnedPiece);
    }

    public void SpawnEnemy(int hpValue, int damageValue, float modelScale, float distanceCoefficient = 1)
    {
        int newZPos = (int)((Random.Range(minDistanceBetweenEnemies, maxDistanceBetweenEnemies) + lastSpawnedEnemyZPos) * distanceCoefficient);

        var newEnemyRotation = Quaternion.Euler(0, 180, 0);
        var enemy = Instantiate(enemyPrefab, Vector3.zero, newEnemyRotation, enemiesContainer);

        var enemyComponent = enemy.GetComponent<DungeonEnemy>();
        enemyComponent.Initialization();

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
        SpawnEnemy(bossHealth, bossDamage, bossScale, 1.5f);
        DungeonManager.Instance.bossZPosition = lastSpawnedEnemyZPos;
    }

    private void SpawnSecondWaveEnemies()
    {
        for (int i = 0; i < secondWaveEnemiesCount; i++)
        {
            SpawnEnemy(Random.Range(minEnemiesHealth, maxEnemiesHealth), Random.Range(minEnemiesDamage, maxEnemiesDamage), 1.5f);
        }
    }

    private void SpawnFirstWaveEnemies()
    {
        for (int i = 0; i < firstWaveEnemiesCount; i++)
        {
            SpawnEnemy(Random.Range(minEnemiesHealth, maxEnemiesHealth), Random.Range(minEnemiesDamage, maxEnemiesDamage), 1.5f);
        }
    }

    private void SpawnDungeonPieces()
    {
        for (int i = 0; i <= startDungeonPiecesCount; i++)
        {
            SpawnPiece();
        }
    }
}