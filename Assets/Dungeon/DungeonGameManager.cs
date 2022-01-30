using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGameManager : MonoBehaviour
{
    [Header("Interactions settings")]
    public static DungeonGameManager dungeonGameManager;
    public DungeonManager dungeonManager;
    

    [Header("Loading settings")]
    public Transform playerStartPosition;
    public Transform playerCharacter;

    public int currentDungeonNumber;
    public void Awake()
    {
        dungeonGameManager = this;
        dungeonManager = GetComponent<DungeonManager>();
    }

    public void Start()
    {
        UploadNewPiece();
    }

    [ContextMenu("Test")]
    public void UploadNewPiece()
    {
        currentDungeonNumber = Random.Range(0, dungeonManager.dungeonsItems.Count);

        playerCharacter.position = playerStartPosition.position;
        Character.character.targetPosition = playerStartPosition.position;
        Character.character.agent.SetDestination(playerStartPosition.position);

        Debug.Log("Player - " + playerCharacter.position + " | Start position - " + playerStartPosition);
        dungeonManager.showSavedNumber = currentDungeonNumber;

        
        dungeonManager.ShowDungeon(); 
    }

}
