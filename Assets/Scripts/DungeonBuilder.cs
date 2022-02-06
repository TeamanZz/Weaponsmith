using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBuilder : MonoBehaviour
{
    public static DungeonBuilder Instance;

    public int enemyChance;
    public Transform piecesParent;
    public GameObject enemyPrefab;
    public List<GameObject> dungeonPieces = new List<GameObject>();
    public List<GameObject> dungeonPiecesPrefabs = new List<GameObject>();

    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private GameObject piecesContainer;

    private GameObject lastSpawnedPiece;
    private int lastSpawnedPieceZPos;
    private int lastSpawnedEnemyZPos;

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

        if (Random.Range(0, enemyChance) == 0)
        {
            var enemy = Instantiate(enemyPrefab, new Vector3(0, 0, lastSpawnedPieceZPos), Quaternion.Euler(0, 180, 0), transform);
            enemy.transform.localPosition = new Vector3(0, 0, lastSpawnedPiece.transform.position.z);
            lastSpawnedEnemyZPos = lastSpawnedPieceZPos;
        }
    }
}