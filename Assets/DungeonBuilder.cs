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
    public int lastSpawnedPieceZPos;
    public Transform piecesParent;

    public void SpawnPiece()
    {
        var newPiece = Instantiate(dungeonPiecesPrefabs[Random.Range(0, dungeonPiecesPrefabs.Count)], new Vector3(0, 0, lastSpawnedPieceZPos), Quaternion.identity, piecesParent);
        newPiece.transform.localPosition = new Vector3(0, 0, newPiece.transform.position.z);
        lastSpawnedPieceZPos += 5;
    }
}