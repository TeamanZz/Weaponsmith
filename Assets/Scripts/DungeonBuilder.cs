using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBuilder : MonoBehaviour
{
    public static DungeonBuilder Instance;

    [Header("Dungeon Pieces Spawn")]
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private Transform piecesContainer;

    [Header("Enemy Spawn")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemiesContainer;

    [SerializeField] private int firstWaveEnemiesCount;
    [SerializeField] private int secondWaveEnemiesCount;

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
        SpawnDungeonPieces();
        SpawnFirstWaveEnemies();
        SpawnChestWithGold();
        SpawnSecondWaveEnemies();
        SpawnBoss();
        SpawnChestWithBlueprint();
    }

    private void SpawnChestWithGold() { }
    private void SpawnChestWithBlueprint() { }

    public void SpawnChest()
    {
        GameObject chestObject = Instantiate(chestPrefab, Vector3.zero, Quaternion.identity, piecesContainer);
        DungeonChest chestComponent = chestObject.GetComponent<DungeonChest>();

        chestComponent.Initialization(DungeonChest.ChestFilling.Money);
        // chestComponent.TakeReward();
        //CHEST AFTER 1 WAVE
        DungeonRewardPanel.Instance.AddItem(DungeonRewardPanel.Instance.moneySprite, chestComponent.currentMoneyReward.ToString());

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

    public void SpawnEnemy(int hpValue, float modelScale)
    {
        var newEnemyPos = new Vector3(0, 0, lastSpawnedEnemyZPos);
        var newEnemyRotation = Quaternion.Euler(0, 180, 0);
        GameObject enemy = Instantiate(enemyPrefab, newEnemyPos, newEnemyRotation, enemiesContainer);

        var enemyHealthComponent = enemy.GetComponent<EnemyHealthBar>();
        enemyHealthComponent.InitializeHP(hpValue);

        var enemyComponent = enemy.GetComponent<DungeonEnemy>();
        enemyComponent.Initialization();

        enemy.transform.localPosition = new Vector3(0, 0, lastSpawnedEnemyZPos);
        enemy.transform.localScale = Vector3.one * modelScale;

        lastSpawnedEnemyZPos += 5;
    }



    private void SpawnBoss()
    {
        SpawnEnemy(bossHealth, bossScale);
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
        for (int i = 0; i <= 30; i++)
        {
            SpawnPiece();
        }
    }
}