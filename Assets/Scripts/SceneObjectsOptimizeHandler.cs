using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectsOptimizeHandler : MonoBehaviour
{
    public static SceneObjectsOptimizeHandler Instance;

    public GameObject firstPieces;
    public GameObject dungeonRoom;
    public GameObject dungeonPreviewScene;
    public DungeonCharacter dungeonCharacter;
    public DungeonManager dungeonManager;

    private GameObject mainRoom;

    private void Awake()
    {
        Instance = this;
        EnableAnvilSceneObjects();
    }

    public void Initialization(GameObject newMainRoom)
    {
        mainRoom = newMainRoom;
        EnableAnvilSceneObjects();
    }
    public void EnableDungeonSceneObjects()
    {
        dungeonRoom.SetActive(true);
        if (dungeonManager.isDungeonStarted)
        {
            dungeonCharacter.animator.enabled = true;
            dungeonCharacter.EnableCharacterRun();
            dungeonPreviewScene.SetActive(false);
        }
        else
        {
            firstPieces.SetActive(false);
            dungeonPreviewScene.SetActive(true);
        }
        mainRoom.SetActive(false);
    }

    public void EnableFirstPieces()
    {
        firstPieces.SetActive(true);
    }

    public void DisablePreviewScene()
    {
        dungeonPreviewScene.SetActive(false);
    }

    public void EnableAnvilSceneObjects()
    {
        dungeonRoom.SetActive(false);
        dungeonCharacter.DisableCharacterRun();
        if (dungeonManager.isDungeonStarted)
            dungeonCharacter.animator.enabled = false;
        mainRoom.SetActive(true);
    }
}