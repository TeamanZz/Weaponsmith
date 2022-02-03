using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;

    public void ToggleSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void DeleteAllProgress()
    {
        MoneyHandler.Instance.AllAwakeInitialization();
        Debug.Log("Start Clear");
        if (MoneyHandler.Instance != null)
            MoneyHandler.Instance.moneyCount = 0;

        PlayerPrefs.DeleteKey("MoneyPerSecond");
        PlayerPrefs.SetInt("currentWaitingPanelNumber", 0);

        if (ItemsManager.Instance != null)
            for (int i = 0; i < ItemsManager.Instance.panelItemsList.Count; i++)
            {
                PlayerPrefs.DeleteKey("UpgradeItem" + i);
                PlayerPrefs.DeleteKey($"UpgradeItem{i}generalIncreaseValue");
                PlayerPrefs.DeleteKey($"UpgradeItem{i}price");
                PlayerPrefs.DeleteKey($"UpgradeItem{i}increaseValue");
                PlayerPrefs.DeleteKey($"UpgradeItem{i}buysCount");
            }

        if (DungeonItemManager.Instance != null)
            for (int i = 0; i < DungeonItemManager.Instance.panelItemsList.Count; i++)
            {
                PlayerPrefs.DeleteKey("DungeonUpgradeItem" + i);
                PlayerPrefs.DeleteKey($"DungeonUpgradeItem{i}generalIncreaseValue");
                PlayerPrefs.DeleteKey($"DungeonUpgradeItem{i}price");
                PlayerPrefs.DeleteKey($"DungeonUpgradeItem{i}increaseValue");
                PlayerPrefs.DeleteKey($"DungeonUpgradeItem{i}buysCount");
            }

        if (BoostersManager.Instance != null)
            for (int i = 0; i < DungeonItemManager.Instance.panelItemsList.Count; i++)
            {
                PlayerPrefs.DeleteKey("BoostersUpgradeItem" + i);
                PlayerPrefs.DeleteKey($"BoostersUpgradeItem{i}generalIncreaseValue");
                PlayerPrefs.DeleteKey($"BoostersUpgradeItem{i}price");
                PlayerPrefs.DeleteKey($"BoostersUpgradeItem{i}increaseValue");
                PlayerPrefs.DeleteKey($"BoostersUpgradeItem{i}buysCount");
            }

        if (RoomObjectsHandler.Instance != null)
            for (int i = 0; i < RoomObjectsHandler.Instance.roomObjects.Count; i++)
            {
                PlayerPrefs.DeleteKey("WorkshopGameobject" + i);
            }

        PlayerPrefs.SetInt("AwardPanel", 0);
        PlayerPrefs.DeleteKey("MoneyCount");

        Debug.Log("End all clear");
        PlayerPrefs.SetString("needLaunch", "true");
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }
}