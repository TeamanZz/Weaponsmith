using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectsOptimizeHandler : MonoBehaviour
{
    public GameObject dungeonRoom;
    public GameObject mainRoom;
    //[SerializeField] private List<GameObject> dungeonObjects = new List<GameObject>();

    //[SerializeField] private List<GameObject> anvilObjects = new List<GameObject>();

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
        mainRoom.SetActive(false);
        //for (var i = 0; i < dungeonObjects.Count; i++)
        //{
        //    dungeonObjects[i].SetActive(true);
        //}

        //for (var i = 0; i < anvilObjects.Count; i++)
        //{
        //    anvilObjects[i].SetActive(false);
        //}
    }

    public void EnableAnvilSceneObjects()
    {
        dungeonRoom.SetActive(false);
        mainRoom.SetActive(true);
        //for (var i = 0; i < dungeonObjects.Count; i++)
        //{
        //    dungeonObjects[i].SetActive(false);
        //}

        //for (var i = 0; i < anvilObjects.Count; i++)
        //{
        //    anvilObjects[i].SetActive(true);
        //}
    }
}