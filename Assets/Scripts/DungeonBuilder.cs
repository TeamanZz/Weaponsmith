using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBuilder : MonoBehaviour
{
    public static DungeonBuilder Instance;

    [Header("Dungeon Pieces Spawn")]
    public Transform piecesParent;
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private GameObject piecesContainer;

    [Header("Enemy Spawn")]
    public int enemySpawnRate;
    public Transform enemiesContainer;
    public GameObject enemyPrefab;

    private GameObject lastSpawnedPiece;
    private int lastSpawnedPieceZPos;
    private int lastSpawnedEnemyZPos;

    public List<GameObject> enemiesSkinsStats = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i <= 20; i++)
        {
            SpawnPiece();
        }
    }

    public void SpawnDungeonContent()
    {
        SpawnPiece();
        SpawnEnemy();
    }

    public void SpawnPiece()
    {
        lastSpawnedPiece = Instantiate(piecePrefab, new Vector3(0, 0, lastSpawnedPieceZPos), Quaternion.identity, piecesParent);
        lastSpawnedPiece.transform.localPosition = new Vector3(0, 0, lastSpawnedPiece.transform.position.z);
        lastSpawnedPieceZPos += 5;
    }

    private void SpawnEnemy()
    {
        if (lastSpawnedEnemyZPos == lastSpawnedPieceZPos - 5 || lastSpawnedEnemyZPos == lastSpawnedPieceZPos - 10)
            return;

        if (Random.Range(0, enemySpawnRate) == 0)
        {
            GameObject enemy = Instantiate(enemyPrefab, new Vector3(0, 0, lastSpawnedPieceZPos), Quaternion.Euler(0, 180, 0), enemiesContainer);

            var enemyHealthComponent = enemy.GetComponent<EnemyHealthBar>();
            enemyHealthComponent.InitializeHP(Random.Range(2, 6));

            enemy.transform.localPosition = new Vector3(0, 0, lastSpawnedPiece.transform.position.z);
            enemy.transform.localScale = Vector3.one * 1.5f;
            lastSpawnedEnemyZPos = lastSpawnedPieceZPos;
        }
    }
}