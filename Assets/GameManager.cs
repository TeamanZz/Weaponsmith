using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public static Canvas mainCanvas;
    public Canvas canvas;
    public static Camera mainCamera;
    public Camera camera;

    public List<GameObject> newObjects = new List<GameObject>();

    public void Awake()
    {
        gameManager = this;

        mainCanvas = canvas;
        mainCamera = camera;
    }

    public void NewClient()
    {
        if (ItemsPanel.itemsPanel.bodyItems.Count > 0 && ItemsPanel.itemsPanel.tipItems.Count > 0)
        {

        }
    }

    public void RemoveObject()
    {
        foreach(GameObject obj in newObjects)
        {
            Destroy(obj);
        }

        newObjects.Clear();
    }
}
