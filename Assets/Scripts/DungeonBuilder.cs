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
        for (int i = 0; i <= 30; i++)
        {
            SpawnPiece();
        }
        lastSpawnedPieceZPos = 0;
        for (int i = 0; i <= firstWaveEnemiesCount; i++)
        {
            SpawnHostileObject();
        }
    }

    private void SpawnChestWithGold() { }

    public void SpawnDungeonContent()
    {
        SpawnPiece();
        SpawnHostileObject();
    }

    public void SpawnPiece()
    {
        lastSpawnedPiece = Instantiate(piecePrefab, new Vector3(0, 0, lastSpawnedPieceZPos), Quaternion.identity, piecesContainer);
        lastSpawnedPiece.transform.localPosition = new Vector3(0, 0, lastSpawnedPiece.transform.position.z);
        lastSpawnedPieceZPos += 5;
    }

    private void SpawnHostileObject()
    {
        // if (lastSpawnedEnemyZPos == lastSpawnedPieceZPos - 5 || lastSpawnedEnemyZPos == lastSpawnedPieceZPos - 10)
        //     return;

        if (needSpawnBoss)
            SpawnEnemy(bossHealth, bossScale);
        else
            SpawnEnemy(Random.Range(2, 6), 1.5f);
    }

    public void SpawnEnemy(int hpValue, float modelScale)
    {
        var newEnemyPos = new Vector3(0, 0, lastSpawnedPieceZPos);
        var newEnemyRotation = Quaternion.Euler(0, 180, 0);
        GameObject enemy = Instantiate(enemyPrefab, newEnemyPos, newEnemyRotation, enemiesContainer);

        var enemyHealthComponent = enemy.GetComponent<EnemyHealthBar>();
        enemyHealthComponent.InitializeHP(hpValue);

        var enemyData = enemy.GetComponent<DungeonEnemy>();
        enemyData.Initialization();

        enemy.transform.localPosition = new Vector3(0, 0, lastSpawnedPiece.transform.position.z);
        enemy.transform.localScale = Vector3.one * modelScale;
        lastSpawnedEnemyZPos = lastSpawnedPieceZPos;
    }
}