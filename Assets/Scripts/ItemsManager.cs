using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager Instance;

    public List<PanelItem> panelItemsList = new List<PanelItem>();

    private void Awake()
    {
        Instance = this;
    }

    public void CheckConditions(PanelItem panelItem)
    {
        for (int i = 0; i < panelItemsList.Count; i++)
        {
            if (panelItemsList[i] != panelItem)
                panelItemsList[i].CheckConditions(panelItem);
        }
    }

    public void MakeNextUnknownItemAsUnavailable()
    {
        //WaitingForDrawing
        var nextItem = panelItemsList.Find(x => x.currentState == PanelItemState.Unknown);
        if (nextItem != null)
            nextItem.ChangeState(PanelItemState.WaitingForDrawing);

    }
}