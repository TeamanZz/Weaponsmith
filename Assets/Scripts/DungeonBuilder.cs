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
    public Transform dungeonCharacterStartPos;
    public TrackingCamera dungeonCamera;
    public Transform dungeonCameraPreviewPos;
    public Transform dungeonCameraStartPos;

    private GameObject lastSpawnedPiece;
    private int lastSpawnedPieceZPos;
    private int lastSpawnedEnemyZPos;

    public bool isDungeonStarted;

    [Header("Chest Settings")]
    public int currentDropRate = 0;
    public int maxDropRate = 5;

    public int currentStarValue = 0;
    private void Awake()
    {
        Instance = this;
        currentDropRate = maxDropRate;
    }

    private void Start()
    {
        for (int i = 0; i <= 20; i++)
        {
            SpawnPiece();
        }
    }

    public void SetCharacterOnStart()
    {
        dungeonCharacter.transform.position = dungeonCharacterStartPos.position;
        dungeonCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);

        dungeonCamera.transform.position = dungeonCameraStartPos.position;
        dungeonCamera.transform.rotation = Quaternion.Euler(32, 0, 0);
        dungeonCamera.targetDistance = 17;

        isDungeonStarted = true;
    }

    public void ShowCharacterPreview()
    {
        if (isDungeonStarted)
            return;
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

            DungeonEnemy enemydata = enemy.GetComponent<DungeonEnemy>();
            enemydata.Initialization();

            enemy.transform.localPosition = new Vector3(0, 0, lastSpawnedPiece.transform.position.z);
            enemy.transform.localScale = Vector3.one * 1.5f;
            lastSpawnedEnemyZPos = lastSpawnedPieceZPos;
        }
    }


}