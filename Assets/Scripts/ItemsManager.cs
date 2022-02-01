using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager Instance;

    public List<PanelItem> panelItemsList = new List<PanelItem>();

    public PanelItem currentWaitingPanel;

    public GameObject newWeaponUnlockPanel;
    private WeaponUnlockPanel weaponPanel;

    [ContextMenu("Awake")]
    private void Awake()
    {
        Instance = this;
        weaponPanel = newWeaponUnlockPanel.GetComponent<WeaponUnlockPanel>();
        newWeaponUnlockPanel.SetActive(false);
        currentWaitingPanel = panelItemsList[PlayerPrefs.GetInt("currentWaitingPanelNumber")];
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

    //  dungeon
    [ContextMenu("Test")]
    public void OpenWaitingPanel()
    {
        if(currentWaitingPanel != null)
            currentWaitingPanel.ChangeState(PanelItemState.Available);

        if (newWeaponUnlockPanel != null && currentWaitingPanel != null)
        {
            newWeaponUnlockPanel.SetActive(true);
            weaponPanel.InitializationPanel(currentWaitingPanel.weaponSprite, currentWaitingPanel.itemName);
            
            if(currentWaitingPanel.currentWeaponNumber < EquipmentManager.equipmentManager.weaponList.Count)
                EquipmentManager.equipmentManager.ShowWeaponsByNumber(currentWaitingPanel.currentWeaponNumber);
        }


    }

    //  ui
    public void ClosedPanel()
    {
        newWeaponUnlockPanel.SetActive(false);
    }

}
