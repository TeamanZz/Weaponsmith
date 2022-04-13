using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectsOptimizeHandler : MonoBehaviour
{
    public GameObject dungeonRoom;
    public DungeonCharacter dungeonCharacter;
    public DungeonManager dungeonManager;
    private GameObject mainRoom;

    private void Awake()
    {
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
        }
        mainRoom.SetActive(false);
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