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

    [Header("Preview")]
    public Transform dungeonCharacter;
    public Transform dungeonCharacterPreviewPos;
    public TrackingCamera dungeonCamera;
    public Transform dungeonCameraPreviewPos;

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

    public void ShowCharacterPreview()
    {
        dungeonCharacter.transform.position = dungeonCharacterPreviewPos.position;
        dungeonCharacter.transform.rotation = Quaternion.Euler(0, -90, 0);

        dungeonCamera.transform.position = dungeonCameraPreviewPos.position;
        dungeonCamera.transform.rotation = Quaternion.Euler(7, 90, 0);
        dungeonCamera.targetDistance = 0;
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