using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PrefsHandler : MonoBehaviour
{
    public static PrefsHandler Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadMoney();
        LoadWorkshopItems();
        LoadUpgrades();
        InvokeRepeating("SaveMoney", 1, 3);
    }

    private void SaveMoney()
    {
        PlayerPrefs.SetString("MoneyCount", MoneyHandler.Instance.moneyCount.ToString());
    }

    private void LoadMoney()
    {
        MoneyHandler.Instance.moneyCount = long.Parse(PlayerPrefs.GetString("MoneyCount", "0"));
    }

    private void LoadWorkshopItems()
    {
        for (int i = 0; i < RoomObjectsHandler.Instance.roomObjects.Count; i++)
        {
            string state = PlayerPrefs.GetString("WorkshopGameobject" + i);
            if (state == "unlocked")
            {
                RoomObjectsHandler.Instance.workshopPanelItems[i].CollapseItemView();
                RoomObjectsHandler.Instance.UnlockObjectWithoutParticles(i);
            }
        }
    }

    private void LoadUpgrades()
    {
        for (int i = 0; i < ItemsManager.Instance.panelItemsList.Count; i++)
        {
            string value = PlayerPrefs.GetString("UpgradeItem" + i);
            if (value == "collapsed")
            {
                ItemsManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.Collapsed);
            }

            if (value == "unknown")
            {
                ItemsManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.Unknown);
            }

            if (value == "WaitingForDrawing")
            {
                ItemsManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.WaitingForDrawing);
            }

            if (value == "available")
            {
                ItemsManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.Available);
            }
        }

        for (int i = 0; i < DungeonItemManager.Instance.panelItemsList.Count; i++)
        {
            string value = PlayerPrefs.GetString("DungeonUpgradeItem" + i);
            if (value == "collapsed")
            {
                DungeonItemManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.Collapsed);
            }

            if (value == "unknown")
            {
                DungeonItemManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.Unknown);
            }

            if (value == "available")
            {
                DungeonItemManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.Available);
            }
        }

        for (int i = 0; i < BoostersManager.Instance.panelItemsList.Count; i++)
        {
            string value = PlayerPrefs.GetString("BoostersUpgradeItem" + i);
            if (value == "collapsed")
            {
                BoostersManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.Collapsed);
            }

            if (value == "unknown")
            {
                BoostersManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.Unknown);
            }

            if (value == "available")
            {
                BoostersManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.Available);
            }
        }
    }
}