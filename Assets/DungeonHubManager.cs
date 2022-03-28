using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonHubManager : MonoBehaviour
{
    public static DungeonHubManager dungeonHubManager;
    public List<GameObject> equipmentPanelContent = new List<GameObject>();

    public Transform armorContent;
    public Transform weaponContent;

    public PanelItemInHub panelInHubPrefab;
    public void Awake()
    {
        dungeonHubManager = this;
    }

    public void AddEquipment(PanelItem newPunelItem)
    {
        Debug.Log("Start Add Equipment");
        if (newPunelItem.currentEquipmentType == ItemEquipment.EquipmentType.Empty)
            return;

        Debug.Log("Add Equipment");
        PanelItemInHub newPanelItemInHub = null;
        ////newPanelItemInHub.panelItem = newPunelItem;
        //newPanelItemInHub.Initialization(newPunelItem);


        switch (newPunelItem.currentEquipmentType)
        {
            case ItemEquipment.EquipmentType.Weapon:
                newPanelItemInHub = Instantiate(panelInHubPrefab, weaponContent);
                newPanelItemInHub.transform.parent = weaponContent;
                break;

            case ItemEquipment.EquipmentType.Armor:
                newPanelItemInHub = Instantiate(panelInHubPrefab, weaponContent);
                newPanelItemInHub.transform.parent = armorContent;
                break;
        }

        newPanelItemInHub.Initialization(newPunelItem);
        newPunelItem.currentPanelItemInHub = newPanelItemInHub;
        Debug.Log(newPanelItemInHub);
    }
    public void OpenPanel(int panelIndex)
    { 
        for(int i = 0; i < equipmentPanelContent.Count; i++)
        {
            equipmentPanelContent[i].SetActive(false);
        }

        switch(panelIndex)
        {
            case 3:
                PanelsHandler.Instance.EnableDungeonEnteringPanel();
                break;

            default:
                if (panelIndex >= equipmentPanelContent.Count)
                    break;

                equipmentPanelContent[panelIndex].SetActive(true);
                break;
        }
    }
}
