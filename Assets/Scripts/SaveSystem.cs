using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public List<PanelItem> items = new List<PanelItem>();
    public List<WorkshopItem> workshopItems = new List<WorkshopItem>();

    [ContextMenu("Remove Panels Data")]
    public void RemovePanelsData()
    {
        foreach (var item in items)
            item.RemoveData();
    }

    [ContextMenu("Remove Rooms Data")]
    public void RemoveRoomssData()
    {
        foreach (var item in workshopItems)
            item.RemoveData();
    }
}
