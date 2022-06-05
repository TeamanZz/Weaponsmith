using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftPanelItemsManager : MonoBehaviour
{
    public static CraftPanelItemsManager Instance;
    public List<PanelItem> craftPanelItemsList = new List<PanelItem>();

    public void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            OpenNewBlueprint();
        }
    }

    public void Initialization(List<PanelItem> newPanelItems)
    {
        craftPanelItemsList.Clear();
        craftPanelItemsList.AddRange(newPanelItems);
    }

    [ContextMenu("Open new blueprint")]
    public void OpenNewBlueprint()
    {
        var nextItem = craftPanelItemsList.Find(x => x.currentState == PanelItemState.Unknown);
        if (nextItem != null)
            nextItem.ChangeState(PanelItemState.Available);
        // EquipmentManager.Singleton.ShowWeaponsByNumber();
    }
}