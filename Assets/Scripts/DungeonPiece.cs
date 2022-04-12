using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPiece : MonoBehaviour
{
    private DungeonCharacter dungeonCharacter;
    [SerializeField] private List<GameObject> piecesList = new List<GameObject>();

    private void Start()
    {
        piecesList[Random.Range(0, piecesList.Count)].SetActive(true);
    }

    private void FixedUpdate()
    {
        if (dungeonCharacter != null && dungeonCharacter.transform.position.z > transform.position.z && Vector3.Distance(dungeonCharacter.transform.position, transform.position) > 20)
        {
            Destroy(gameObject);
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     DungeonCharacter dungeonCharacterTemp;

    //     if (other.TryGetComponent<DungeonCharacter>(out dungeonCharacterTemp))
    //     {
    //         dungeonCharacter = dungeonCharacterTemp;
    //         DungeonBuilder.Instance.SpawnDungeonContent();
    //     }
    // }
}
