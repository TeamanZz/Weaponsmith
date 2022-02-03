using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPiece : MonoBehaviour
{
    private DungeonCharacter dungeonCharacter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<DungeonCharacter>(out dungeonCharacter))
        {
            DungeonBuilder.Instance.SpawnPiece();
        }
    }
}
