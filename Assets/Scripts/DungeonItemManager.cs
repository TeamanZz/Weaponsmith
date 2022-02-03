using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonItemManager : MonoBehaviour
{
    public static DungeonItemManager Instance;

    public List<DungeonPanelItem> panelItemsList = new List<DungeonPanelItem>();

    [ContextMenu("Awake")]
    public void Awake()
    {
        Instance = this;
    }
    public void MakeNextUnknownItemAsUnavailable()
    {
        //WaitingForDrawing
        var nextItem = panelItemsList.Find(x => x.currentState == PanelItemState.Unknown);
        if (nextItem != null)
            nextItem.ChangeState(PanelItemState.WaitingForDrawing);

    }

}