using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPanelItemsManager : MonoBehaviour
{
    public static DungeonPanelItemsManager Instance;

    [HideInInspector] public List<DungeonPanelItem> panelItemsList = new List<DungeonPanelItem>();

    [ContextMenu("Awake")]
    public void Awake()
    {
        Instance = this;
    }

    public void Initialization(List<DungeonPanelItem> newPanels)
    {
        panelItemsList.Clear();
        panelItemsList.AddRange(newPanels);
    }

    public void MakeNextUnknownItemAsUnavailable()
    {
        //WaitingForDrawing
        var nextItem = panelItemsList.Find(x => x.currentState == PanelItemState.Unknown);
        if (nextItem != null)
            nextItem.ChangeState(PanelItemState.WaitingForDrawing);
    }
}