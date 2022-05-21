using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem saveSystem;

    public List<PanelItem> items = new List<PanelItem>();
    public List<WorkshopItem> workshopItems = new List<WorkshopItem>();
    public List<PanelItemInHub> hubItem = new List<PanelItemInHub>();

    public void Awake()
    {
        saveSystem = this;    
    }

    [ContextMenu("Remove All Data")]
    public void RemoveAllData()
    {
        RemoveHubPanelData();
        RemovePanelsData();
        RemoveRoomsData();
    }

    [ContextMenu("Remove Panels Data")]
    public void RemovePanelsData()
    {
        foreach (var item in items)
            item.RemoveData();
    }

    [ContextMenu("Remove Rooms Data")]
    public void RemoveRoomsData()
    {
        foreach (var item in workshopItems)
            item.RemoveData();
    }

    [ContextMenu("Remove Hub Data")]
    public void RemoveHubPanelData()
    {
        foreach (var item in workshopItems)
            item.RemoveData();
        hubItem.Clear();
    }
} 
