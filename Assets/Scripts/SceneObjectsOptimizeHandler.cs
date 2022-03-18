using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectsOptimizeHandler : MonoBehaviour
{
    public GameObject dungeonRoom;
    public DungeonCharacter dungeonCharacter;
    public DungeonBuilder dungeonBuilder;
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
        if (dungeonBuilder.isDungeonStarted)
        {
            dungeonCharacter.animator.enabled = true;

            dungeonCharacter.runSpeed = 0.1f;
        }
        mainRoom.SetActive(false);
    }

    public void EnableAnvilSceneObjects()
    {
        dungeonRoom.SetActive(false);
        dungeonCharacter.runSpeed = 0;
        if (dungeonBuilder.isDungeonStarted)
            dungeonCharacter.animator.enabled = false;
        mainRoom.SetActive(true);
    }
}