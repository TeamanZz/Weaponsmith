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

    [SerializeField] private GameObject chestPrefab;

    private GameObject lastSpawnedPiece;
    private int lastSpawnedPieceZPos;
    private int lastSpawnedEnemyZPos;
    private bool needSpawnBoss;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        BuildDungeon();
    }

    private void BuildDungeon()
    {
        // SpawnDungeonPieces();
        // SpawnFirstWaveEnemies();
        // SpawnChestWithGold();
        // SpawnSecondWaveEnemies();
        // SpawnBoss();
        // SpawnChestWithBlueprint();

        SpawnDungeonPieces();
        SpawnChestWithGold();
    }

    private void SpawnChestWithGold()
    {
        int newZPos = Random.Range(minDistanceBetweenEnemies, maxDistanceBetweenEnemies) + lastSpawnedEnemyZPos;

        GameObject chestObject = Instantiate(chestPrefab, Vector3.zero, Quaternion.identity, piecesContainer);
        DungeonChest chestComponent = chestObject.GetComponent<DungeonChest>();
        chestComponent.Initialization(DungeonChest.ChestFilling.Money);
        DungeonRewardPanel.Instance.AddItem(DungeonRewardPanel.Instance.moneySprite, chestComponent.currentMoneyReward.ToString());

        chestObject.transform.localPosition = new Vector3(0, 0, newZPos);
        lastSpawnedEnemyZPos = newZPos;
    }

    private void SpawnChestWithBlueprint()
    {
        // GameObject chestObject = Instantiate(chestPrefab, Vector3.zero, Quaternion.identity, piecesContainer);
        // DungeonChest chestComponent = chestObject.GetComponent<DungeonChest>();
        //CHEST AFTER BOSS
        // chestComponent.Initialization(DungeonWeaponBlueprint.ChestFilling.drawing, rewardStars);
        // DungeonRewardPanel.dungeonRewardPanel.AddItem(DungeonRewardPanel.dungeonRewardPanel.armorSprite, "New Blueprint");
        // DungeonRewardPanel.dungeonRewardPanel.AddItem(DungeonRewardPanel.dungeonRewardPanel.weaponSprite, "New Blueprint");
    }

    public void SpawnPiece()
    {
        lastSpawnedPiece = Instantiate(piecePrefab, new Vector3(0, 0, lastSpawnedPieceZPos), Quaternion.identity, piecesContainer);
        lastSpawnedPiece.transform.localPosition = new Vector3(0, 0, lastSpawnedPiece.transform.position.z);
        lastSpawnedPieceZPos += 5;
    }

    public void SpawnEnemy(int hpValue, float modelScale, float distanceCoefficient = 1)
    {
        int newZPos = (int)((Random.Range(minDistanceBetweenEnemies, maxDistanceBetweenEnemies) + lastSpawnedEnemyZPos) * distanceCoefficient);

        var newEnemyRotation = Quaternion.Euler(0, 180, 0);
        var enemy = Instantiate(enemyPrefab, Vector3.zero, newEnemyRotation, enemiesContainer);

        var enemyComponent = enemy.GetComponent<DungeonEnemy>();
        enemyComponent.Initialization();

        var enemyHealthComponent = enemy.GetComponent<EnemyHealthBar>();
        enemyHealthComponent.InitializeHP(hpValue);

        enemy.transform.localPosition = new Vector3(0, 0, newZPos);
        enemy.transform.localScale = Vector3.one * modelScale;

        lastSpawnedEnemyZPos = newZPos;
    }

    private void SpawnBoss()
    {
        SpawnEnemy(bossHealth, bossScale, 1.5f);
    }

    private void SpawnSecondWaveEnemies()
    {
        for (int i = 0; i <= secondWaveEnemiesCount; i++)
        {
            SpawnEnemy(Random.Range(2, 6), 1.5f);
        }
    }

    private void SpawnFirstWaveEnemies()
    {
        for (int i = 0; i <= firstWaveEnemiesCount; i++)
        {
            SpawnEnemy(Random.Range(2, 6), 1.5f);
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