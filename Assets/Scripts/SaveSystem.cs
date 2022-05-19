using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public List<PanelItem> items = new List<PanelItem>();

    public void RemoveAllData()
    {
        foreach (var item in items)
            item.RemoveData();
    }
}
