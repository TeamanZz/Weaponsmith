using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("View settings")]
    public Animator cameraAnimator;

    [Header("Main settings")]
    public static GameObject mainTable;
    public GameObject table;
    public Transform craftPoint;
    public Transform exportPoint;

    public static GameManager gameManager;
    public static Canvas mainCanvas;
    public Canvas canvas;
    public static Camera mainCamera;
    public Camera camera;

    public List<GameObject> newObjects = new List<GameObject>();

    [Header("Items to compare")]
    public Item bodyItem;
    public Item tipItem;
    public Item accessoryItems;

    public Item resultBody;
    public Item resultTip;
    public Item resultAccesory;

    [Header("Preview UI settings")]
    public ClientAI setAi;
    public GameObject previewPanel;
    public GameObject itemsBar;
    public Transform previewPoint;
    public GameObject previewObject;

    public void Awake()
    {
        gameManager = this;

        mainTable = table;
        mainCanvas = canvas;
        mainCamera = camera;

        previewPanel.SetActive(false);
        itemsBar.SetActive(false);
    }

    [ContextMenu("Test")]
    public void NewClient()
    {
        RemoveObject();
        if (ItemsPanel.itemsPanel.bodyItems.Count > 0 && ItemsPanel.itemsPanel.tipItems.Count > 0)
        {
            bodyItem = ItemsPanel.itemsPanel.bodyItems[Random.Range(0, ItemsPanel.itemsPanel.bodyItems.Count - 1)];
            tipItem = ItemsPanel.itemsPanel.tipItems[Random.Range(0, ItemsPanel.itemsPanel.tipItems.Count - 1)];

            if (ItemsPanel.itemsPanel.accessoryItems.Count > 0)
                accessoryItems = ItemsPanel.itemsPanel.accessoryItems[Random.Range(0, ItemsPanel.itemsPanel.accessoryItems.Count - 1)];

            InitializationResultPreview();

            cameraAnimator.SetBool("cameraMove", true);
        }
    }

    public void InitializationResultPreview()
    {
        previewPanel.SetActive(true);
        itemsBar.SetActive(true);

        if (bodyItem != null)
        {
            previewObject = Instantiate(bodyItem.itemPrefab, previewPoint);
            previewObject.transform.parent = previewObject.transform;
            previewObject.transform.localScale = bodyItem.previewScale;
            previewObject.transform.localRotation = Quaternion.Euler(bodyItem.previewRotation);
        }
        if (tipItem != null)
        {
            if (previewObject.transform.GetComponentInChildren<SphereCollider>() != null)
            {
                Transform tipPoint = previewObject.transform.GetComponentInChildren<SphereCollider>().transform;
                GameObject prevTip = Instantiate(tipItem.itemPrefab, tipPoint);

                prevTip.transform.parent = tipPoint.transform;
                prevTip.transform.localScale = new Vector3(1f, 1f, 1f);
                previewObject.transform.localScale *= tipItem.previewScale.z;
                prevTip.transform.localRotation = Quaternion.Euler(Vector3.zero);
                //prevTip.transform.parent = tipPoint.transform;
                //prevTip.transform.localScale = bodyItem.previewScale;
                //prevTip.transform.localRotation = Quaternion.Euler(bodyItem.previewRotation);
            }
        }
    }

    public void RemoveObject()
    {
        foreach (GameObject obj in newObjects)
        {
            Destroy(obj);
        }

        if (previewPoint != null)
        {
            if (previewPoint.childCount > 0)
            {
                Debug.Log(previewPoint.GetChild(0).gameObject);
                Destroy(previewPoint.GetChild(0).gameObject);
            }
        }

        resultBody = null;
        resultTip = null;

        newObjects.Clear();
    }

    [ContextMenu("Result")]
    public void ResultCalculation()
    {
        bool react;
        cameraAnimator.SetBool("cameraMove", false);

        previewPanel.SetActive(false);
        itemsBar.SetActive(false);

        if (resultBody == null || resultBody == null)
        {
            setAi.FeedBack(false);
            RemoveObject();
        }

        if (bodyItem.itemPrefab.gameObject == resultBody.itemPrefab.gameObject)
        {
            if (tipItem.itemPrefab.gameObject == resultBody.itemPrefab.gameObject)
            {
                Debug.Log("Good result");
                react = true;
                Debug.Log(bodyItem.itemPrefab.gameObject + " | " + resultBody.itemPrefab.gameObject);
                Debug.Log(tipItem.itemPrefab.gameObject + " | " + resultTip.itemPrefab.gameObject);
            }
            else
                react = false;
        }
        else
        {
            Debug.Log("Bad result");
            react = false;
        }


        setAi.FeedBack(react);
        RemoveObject();
    }
}
