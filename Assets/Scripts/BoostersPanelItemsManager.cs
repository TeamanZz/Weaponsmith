using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostersPanelItemsManager : MonoBehaviour
{
    public static BoostersPanelItemsManager Instance;

    [HideInInspector] public List<BoostersItemPanel> panelItemsList = new List<BoostersItemPanel>();

    [ContextMenu("Awake")]
    public void Awake()
    {
        Instance = this;
    }

    public void Initialization(List<BoostersItemPanel> boostersItems)
    {
        panelItemsList.Clear();
        panelItemsList.AddRange(boostersItems);
    }
    public void MakeNextUnknownItemAsUnavailable()
    {
        //WaitingForDrawing
        var nextItem = panelItemsList.Find(x => x.currentState == PanelItemState.Unknown);
        if (nextItem != null)
            nextItem.ChangeState(PanelItemState.WaitingForDrawing);
    }
}