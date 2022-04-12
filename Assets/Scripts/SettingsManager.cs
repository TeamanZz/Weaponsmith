using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager settingsManager;
    public GameObject settingsPanel;

    public void Awake()
    {
        settingsManager = this;
    }
    public void ToggleSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    [ContextMenu("Delete all data")]
    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }


    public void RemovingOldEraProgress()
    {
        // Debug.Log("Start Clear");
        // if (MoneyHandler.Instance != null)
        //     MoneyHandler.Instance.moneyCount = 0;

        // PlayerPrefs.DeleteKey("MoneyPerSecond");
        // PlayerPrefs.SetInt("currentWaitingPanelNumber", 0);

        // if (CraftPanelItemsManager.Instance != null)
        //     for (int i = 0; i < CraftPanelItemsManager.Instance.craftPanelItemsList.Count; i++)
        //     {
        //         PlayerPrefs.DeleteKey("UpgradeItem" + i);
        //         PlayerPrefs.DeleteKey($"UpgradeItem{i}generalIncreaseValue");
        //         PlayerPrefs.DeleteKey($"UpgradeItem{i}price");
        //         PlayerPrefs.DeleteKey($"UpgradeItem{i}increaseValue");
        //         PlayerPrefs.DeleteKey($"UpgradeItem{i}buysCount");
        //     }

        // if (DungeonPanelItemsManager.Instance != null)
        //     for (int i = 0; i < DungeonPanelItemsManager.Instance.panelItemsList.Count; i++)
        //     {
        //         PlayerPrefs.DeleteKey("DungeonUpgradeItem" + i);
        //         PlayerPrefs.DeleteKey($"DungeonUpgradeItem{i}generalIncreaseValue");
        //         PlayerPrefs.DeleteKey($"DungeonUpgradeItem{i}price");
        //         PlayerPrefs.DeleteKey($"DungeonUpgradeItem{i}increaseValue");
        //         PlayerPrefs.DeleteKey($"DungeonUpgradeItem{i}buysCount");
        //     }

        // if (BoostersPanelItemsManager.Instance != null)
        //     for (int i = 0; i < DungeonPanelItemsManager.Instance.panelItemsList.Count; i++)
        //     {
        //         PlayerPrefs.DeleteKey("BoostersUpgradeItem" + i);
        //         PlayerPrefs.DeleteKey($"BoostersUpgradeItem{i}generalIncreaseValue");
        //         PlayerPrefs.DeleteKey($"BoostersUpgradeItem{i}price");
        //         PlayerPrefs.DeleteKey($"BoostersUpgradeItem{i}increaseValue");
        //         PlayerPrefs.DeleteKey($"BoostersUpgradeItem{i}buysCount");
        //     }

        // if (WorkshopPanelItemsManager.Instance != null)
        //     for (int i = 0; i < WorkshopPanelItemsManager.Instance.currentEraWorkshopObjects.Count; i++)
        //     {
        //         PlayerPrefs.DeleteKey("WorkshopGameobject" + i);
        //     }

        // PlayerPrefs.SetInt("AwardPanel", 0);
        // PlayerPrefs.DeleteKey("MoneyCount");
        // PlayerPrefs.SetInt("WeaponNumber", 0);
        // PlayerPrefs.DeleteKey("dungeoonIsOpen");
        // PlayerPrefs.DeleteKey("enchantmentIsOpen");
        // PlayerPrefs.DeleteKey("allowedAttackAnimationsCount");
        // PlayerPrefs.DeleteKey("weaponTrailEnabled");
        // PlayerPrefs.DeleteKey("canCriticalHit");

        // PlayerPrefs.SetInt("skinIndex", 0);
        // PlayerPrefs.SetInt("EnemySkinCount", 0);

        // Debug.Log("End all clear");
        // PlayerPrefs.SetString("needLaunch", "true");
        // PlayerPrefs.Save();
        // SceneManager.LoadScene(0);

    }

    //all
    public void DeleteAllProgress()
    {
        Debug.Log("Delete All");
        PlayerPrefs.DeleteAll();
        //Debug.Log("Start Clear");
        //if (MoneyHandler.Instance != null)
        //    MoneyHandler.Instance.moneyCount = 0;

        //PlayerPrefs.DeleteKey("skinIndex");
        //PlayerPrefs.DeleteKey("MoneyPerSecond");
        //PlayerPrefs.SetInt("currentWaitingPanelNumber", 0);
        //PlayerPrefs.DeleteKey("allowedAttackAnimationsCount");
        //PlayerPrefs.DeleteKey("weaponTrailEnabled");
        //PlayerPrefs.DeleteKey("canCriticalHit");

        //if (CraftPanelItemsManager.Instance != null)
        //    for (int i = 0; i < CraftPanelItemsManager.Instance.craftPanelItemsList.Count; i++)
        //    {
        //        PlayerPrefs.DeleteKey("UpgradeItem" + i);
        //        PlayerPrefs.DeleteKey($"UpgradeItem{i}generalIncreaseValue");
        //        PlayerPrefs.DeleteKey($"UpgradeItem{i}price");
        //        PlayerPrefs.DeleteKey($"UpgradeItem{i}increaseValue");
        //        PlayerPrefs.DeleteKey($"UpgradeItem{i}buysCount");
        //    }

        //if (DungeonPanelItemsManager.Instance != null)
        //    for (int i = 0; i < DungeonPanelItemsManager.Instance.panelItemsList.Count; i++)
        //    {
        //        PlayerPrefs.DeleteKey("DungeonUpgradeItem" + i);
        //        PlayerPrefs.DeleteKey($"DungeonUpgradeItem{i}generalIncreaseValue");
        //        PlayerPrefs.DeleteKey($"DungeonUpgradeItem{i}price");
        //        PlayerPrefs.DeleteKey($"DungeonUpgradeItem{i}increaseValue");
        //        PlayerPrefs.DeleteKey($"DungeonUpgradeItem{i}buysCount");
        //    }

        //if (BoostersPanelItemsManager.Instance != null)
        //    for (int i = 0; i < DungeonPanelItemsManager.Instance.panelItemsList.Count; i++)
        //    {
        //        PlayerPrefs.DeleteKey("BoostersUpgradeItem" + i);
        //        PlayerPrefs.DeleteKey($"BoostersUpgradeItem{i}generalIncreaseValue");
        //        PlayerPrefs.DeleteKey($"BoostersUpgradeItem{i}price");
        //        PlayerPrefs.DeleteKey($"BoostersUpgradeItem{i}increaseValue");
        //        PlayerPrefs.DeleteKey($"BoostersUpgradeItem{i}buysCount");
        //    }

        //if (WorkshopPanelItemsManager.Instance != null)
        //    for (int i = 0; i < 30; i++)
        //    {
        //        PlayerPrefs.DeleteKey("WorkshopGameobject" + i);
        //    }

        //PlayerPrefs.SetInt("AwardPanel", 0);
        //PlayerPrefs.DeleteKey("MoneyCount");
        //PlayerPrefs.DeleteKey("dungeoonIsOpen");
        //PlayerPrefs.DeleteKey("enchantmentIsOpen");
        //PlayerPrefs.SetInt("WeaponNumber", 0);
        //PlayerPrefs.SetInt("EnemySkinCount", 0);
        //PlayerPrefs.SetInt("currentEraNumber", 0);

        //Debug.Log("End all clear");
        //PlayerPrefs.SetString("needLaunch", "true");
        //PlayerPrefs.Save();
        //SceneManager.LoadScene(0);
    }
}