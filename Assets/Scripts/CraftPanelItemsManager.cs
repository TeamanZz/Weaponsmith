using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftPanelItemsManager : MonoBehaviour
{
    public static CraftPanelItemsManager Instance;
    public List<PanelItem> craftPanelItemsList = new List<PanelItem>();
    private int awardPanelState = 0;

    [ContextMenu("Awake")]
    public void Awake()
    {
        Instance = this;
    }

    public void Initialization(List<PanelItem> newPanelItems)
    {
        craftPanelItemsList.Clear();
        craftPanelItemsList.AddRange(newPanelItems);
    }

    [ContextMenu("Test")]
    public void OpenNewBlueprint()
    {
        var nextItem = craftPanelItemsList.Find(x => x.currentState == PanelItemState.Unknown);
        if (nextItem != null)
            nextItem.ChangeState(PanelItemState.Available);
        EquipmentManager.Singleton.ShowWeaponsByNumber();
    }
}