using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PrefsHandler : MonoBehaviour
{
    public static PrefsHandler Instance;
    public List<PartsItem> blueprintsList = new List<PartsItem>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // PlayerPrefs.DeleteAll();
        LoadMoney();
        LoadWorkshopItems();
        LoadBlueprints();
        LoadUpgrades();

        InvokeRepeating("SaveMoney", 1, 3);
    }

    private void OnDestroy()
    {
        SaveMoney();
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

    private void LoadBlueprints()
    {
        for (int i = 0; i < blueprintsList.Count; i++)
        {
            if (PlayerPrefs.GetString("BlueprintPanelItem" + i) == "unlocked")
            {
                blueprintsList[i].UnlockItem();
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
            if (value == "unavailable")
            {
                ItemsManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.Unavailable);
            }
            if (value == "available")
            {
                ItemsManager.Instance.panelItemsList[i].ChangeStateViaLoader(PanelItemState.Available);
            }
        }
    }
}