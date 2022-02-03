using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBuilder : MonoBehaviour
{
    public static DungeonBuilder Instance;
    private void Awake()
    {
        Instance = this;
    }

    public List<GameObject> dungeonPieces = new List<GameObject>();
    public List<GameObject> dungeonPiecesPrefabs = new List<GameObject>();
    public GameObject enemyPrefab;
    public int enemyChance;
    public int lastSpawnedPieceZPos;
    public Transform piecesParent;

    public void SpawnPiece()
    {
        var newPiece = Instantiate(dungeonPiecesPrefabs[Random.Range(0, dungeonPiecesPrefabs.Count)], new Vector3(0, 0, lastSpawnedPieceZPos), Quaternion.identity, piecesParent);
        newPiece.transform.localPosition = new Vector3(0, 0, newPiece.transform.position.z);
        lastSpawnedPieceZPos += 5;

        if (Random.Range(0, enemyChance) == 1)
        {
            var enemy = Instantiate(enemyPrefab, new Vector3(0, 0, lastSpawnedPieceZPos), Quaternion.Euler(0, 180, 0), transform);
            enemy.transform.localPosition = new Vector3(0, 0, newPiece.transform.position.z);
        }
    }
}