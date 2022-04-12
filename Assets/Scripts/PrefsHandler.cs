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
        // MoneyHandler.Instance.moneyCount = long.Parse(PlayerPrefs.GetString("MoneyCount", "0"));
        // MoneyHandler.Instance.moneyCount = long.Parse(PlayerPrefs.GetString("MoneyCount", "999999999999999"));
    }

    private void LoadWorkshopItems()
    {
        for (int i = 0; i < WorkshopPanelItemsManager.Instance.currentEraWorkshopObjects.Count; i++)
        {
            string state = PlayerPrefs.GetString("WorkshopGameobject" + i);
            if (state == "unlocked")
            {
                WorkshopPanelItemsManager.Instance.workshopPanelItems[i].CollapseItemView();
                WorkshopPanelItemsManager.Instance.UnlockObjectWithoutParticles(i);
            }
        }
    }

    private void LoadUpgrades()
    {
        for (int i = 0; i < CraftPanelItemsManager.Instance.craftPanelItemsList.Count; i++)
        {
            string value = PlayerPrefs.GetString("UpgradeItem" + i);
            if (value == "collapsed")
            {
                CraftPanelItemsManager.Instance.craftPanelItemsList[i].ChangeStateViaLoader(PanelItemState.Collapsed);
            }

            if (value == "unknown")
            {
                CraftPanelItemsManager.Instance.craftPanelItemsList[i].ChangeStateViaLoader(PanelItemState.Unknown);
            }

            if (value == "WaitingForDrawing")
            {
                CraftPanelItemsManager.Instance.craftPanelItemsList[i].ChangeStateViaLoader(PanelItemState.WaitingForDrawing);
            }

            if (value == "available")
            {
                CraftPanelItemsManager.Instance.craftPanelItemsList[i].ChangeStateViaLoader(PanelItemState.Available);
            }
        }

        for (int i = 0; i < DungeonPanelItemsManager.Instance.panelItemsList.Count; i++)
        {
            string value = PlayerPrefs.GetString("DungeonUpgradeItem" + i);
            if (value == "collapsed")
            {
                DungeonPanelItemsManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.Collapsed);
            }

            if (value == "unknown")
            {
                DungeonPanelItemsManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.Unknown);
            }

            if (value == "available")
            {
                DungeonPanelItemsManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.Available);
            }
        }

        for (int i = 0; i < BoostersPanelItemsManager.Instance.panelItemsList.Count; i++)
        {
            string value = PlayerPrefs.GetString("BoostersUpgradeItem" + i);
            if (value == "collapsed")
            {
                BoostersPanelItemsManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.Collapsed);
            }

            if (value == "unknown")
            {
                BoostersPanelItemsManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.Unknown);
            }

            if (value == "available")
            {
                BoostersPanelItemsManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.Available);
            }
        }
    }
}