using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager Instance;

    public List<PanelItem> panelItemsList = new List<PanelItem>();

    public PanelItem currentWaitingPanel;

    private void Awake()
    {
        Instance = this;
    }

    //public void CheckConditions(PanelItem panelItem)
    //{
    //    for (int i = 0; i < panelItemsList.Count; i++)
    //    {
    //        if (panelItemsList[i] != panelItem)
    //            panelItemsList[i].CheckConditions(panelItem);
    //    }
    //}

    public void MakeNextUnknownItemAsUnavailable()
    {
        //WaitingForDrawing
        var nextItem = panelItemsList.Find(x => x.currentState == PanelItemState.Unknown);
        if (nextItem != null)
            nextItem.ChangeState(PanelItemState.WaitingForDrawing);

    }

    public GameObject panel;
    // dungeon
   [ContextMenu("Test")]
    public void OpenWaitingPanel()
    {
        currentWaitingPanel.ChangeState(PanelItemState.Available);
        if(panel!= null)
            panel.SetActive(true);
    }


    //ui
    public void ClosedPanel()
    {
        panel.SetActive(false);
    }

}
