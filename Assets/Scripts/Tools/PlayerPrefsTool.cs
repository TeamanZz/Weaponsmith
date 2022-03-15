using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerPrefsTool : MonoBehaviour
{
#if UNITY_EDITOR
    // [MenuItem("PlayerPrefs/Clear Player Prefs")]
    // static void ClearPlayerPrefs()
    // {
    //     Debug.Log("Start Clear");
    //     if (MoneyHandler.Instance != null)
    //         MoneyHandler.Instance.moneyCount = 0;

    //     PlayerPrefs.DeleteKey("MoneyPerSecond");
    //     PlayerPrefs.SetInt("currentWaitingPanelNumber", 0);

    //     if (CraftPanelItemsManager.Instance != null)
    //         for (int i = 0; i < CraftPanelItemsManager.Instance.craftPanelItemsList.Count; i++)
    //         {
    //             PlayerPrefs.DeleteKey("UpgradeItem" + i);
    //         }

    //     if (DungeonPanelItemsManager.Instance != null)
    //         for (int i = 0; i < DungeonPanelItemsManager.Instance.panelItemsList.Count; i++)
    //         {
    //             PlayerPrefs.DeleteKey("DungeonUpgradeItem" + i);
    //         }

    //     if (BoostersManager.Instance != null)
    //         for (int i = 0; i < DungeonPanelItemsManager.Instance.panelItemsList.Count; i++)
    //         {
    //             PlayerPrefs.DeleteKey("BoostersUpgradeItem" + i);
    //         }

    //     if (RoomObjectsHandler.Instance != null)
    //         for (int i = 0; i < RoomObjectsHandler.Instance.roomObjects.Count; i++)
    //         {
    //             PlayerPrefs.DeleteKey("WorkshopGameobject" + i);
    //         }

    //     PlayerPrefs.SetInt("AwardPanel", 0);
    //     PlayerPrefs.DeleteKey("MoneyCount");

    //     Debug.Log("End all clear");
    // }
    // // [MenuItem("PlayerPrefs/Add10000PP")]

    // // static void Add10000PP()
    // // {
    // //     var oldValue = PlayerPrefs.GetInt("PopulationPoints");

    // //     oldValue += 10000;
    // //     PlayerPrefs.SetInt("PopulationPoints", oldValue);
    // // }

    // // [MenuItem("PlayerPrefs/Disable FTUE")]
    // static void Temp()
    // {
    //     PlayerPrefs.SetString("WorkshopGameobject" + 0, "heh");
    //     PlayerPrefs.SetString("needLaunch", "stop");
    // }
#endif
}
