using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem saveSystem;
    public SkillController skillController;
    public MoneyHandler moneyHandler;
    public DungeonManager dungeonManager;
    public FTUESystem ftueSystem;

    public PanelsHandler panelsHandler;

    public List<PanelItem> items = new List<PanelItem>();
    public List<WorkshopItem> workshopItems = new List<WorkshopItem>();
    public List<PanelItemInHub> hubItem = new List<PanelItemInHub>();

    public void Awake()
    {
        saveSystem = this;
    }

    [ContextMenu("DELETEALLPREFS")]
    public void DELETEALLPREFS()
    {
        PlayerPrefs.DeleteAll();
    }

    [ContextMenu("Remove All Data")]
    public void RemoveAllData()
    {
        RemovePanelsData();
        RemoveRoomsData();
        RemoveSkillsData();
        RemoveMoneyData();
        RemoveDungeonData();
        RemoveFTUEData();
        panelsHandler.OpenPanel(1);
    }

    public void RemoveDataExceptFTUE()
    {
        RemovePanelsData();
        RemoveRoomsData();
        RemoveSkillsData();
        RemoveMoneyData();
        RemoveDungeonData();
        //RemoveFTUEData();
        panelsHandler.OpenPanel(1);
    }

    //[ContextMenu("Remove Panels Data")]
    private void RemovePanelsData()
    {
        foreach (var item in items)
            item.RemoveData();
    }

    //[ContextMenu("Remove Rooms Data")]
    private void RemoveRoomsData()
    {
        foreach (var item in workshopItems)
            item.RemoveData();
    }

    //[ContextMenu("Remove Hub Data")]
    //private void RemoveHubPanelData()
    //{
    //    foreach (var item in hubItem)
    //        item.RemoveData();
    //    hubItem.Clear();
    //}

    //[ContextMenu("Remove Skills Data")]
    private void RemoveSkillsData()
    {
        skillController.RemoveData();
    }

    //[ContextMenu("Remove Money Data")]
    private void RemoveMoneyData()
    {
        moneyHandler.RemoveData();
    }

    //[ContextMenu("Remove Dungeon Data")]
    private void RemoveDungeonData()
    {
        dungeonManager.RemoveData();
    }

    public void RemoveFTUEData()
    {
        ftueSystem.RemoveData();
    }
}
