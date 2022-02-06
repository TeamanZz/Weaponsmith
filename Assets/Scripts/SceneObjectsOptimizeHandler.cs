using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectsOptimizeHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> dungeonObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> anvilObjects = new List<GameObject>();

    private void Awake()
    {
        EnableAnvilSceneObjects();
    }

    public void EnableDungeonSceneObjects()
    {
        for (var i = 0; i < dungeonObjects.Count; i++)
        {
            dungeonObjects[i].SetActive(true);
        }

        for (var i = 0; i < anvilObjects.Count; i++)
        {
            anvilObjects[i].SetActive(false);
        }
    }

    public void EnableAnvilSceneObjects()
    {
        for (var i = 0; i < dungeonObjects.Count; i++)
        {
            dungeonObjects[i].SetActive(false);
        }

        for (var i = 0; i < anvilObjects.Count; i++)
        {
            anvilObjects[i].SetActive(true);
        }
    }
}