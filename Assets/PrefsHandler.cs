using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsHandler : MonoBehaviour
{
    public List<PartsItem> blueprintsList = new List<PartsItem>();

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
        PlayerPrefs.SetInt("MoneyCount", MoneyHandler.Instance.moneyCount);
    }

    private void LoadMoney()
    {
        MoneyHandler.Instance.moneyCount = PlayerPrefs.GetInt("MoneyCount");
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
                Debug.Log("asasa");
                blueprintsList[i].BuyItemViaPrefs();
            }
        }
    }

    private void LoadUpgrades()
    {
        for (int i = 0; i < ItemsManager.Instance.panelItemsList.Count; i++)
        {
            string value = PlayerPrefs.GetString("UpgradeItem" + i);
            Debug.Log(value);
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
                // Debug.Log("available");
            }
        }
    }
}