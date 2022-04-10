using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonHubManager : MonoBehaviour
{
    public static DungeonHubManager dungeonHubManager;
    
    [Header("Data")]
    public List<GameObject> equipmentPanelContent = new List<GameObject>();

    public Transform armorContent;
    public Transform weaponContent;

    public PanelItemInHub panelInHubPrefab;

    public PanelItemInHub armorActivatePanel;
    public PanelItemInHub weaponActivatePanel;

    public List<GameObject> armorList = new List<GameObject>();
    public List<GameObject> weaponList = new List<GameObject>();

    [Header("UI")]
    public Image armorUIImage;
    public Image weaponUIImage;

    public Sprite armorSprite;
    public Sprite weaponSprite;

    public LoadoutController loadoutController;
    public void Awake()
    {
        dungeonHubManager = this;
        foreach (var currentObj in weaponList)
        {
            currentObj.SetActive(false);
        }
        foreach (var currentObj in armorList)
        {
            currentObj.SetActive(false);
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        armorUIImage.sprite = armorSprite;
        weaponUIImage.sprite = weaponSprite;
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
        for (int i = 0; i < equipmentPanelContent.Count; i++)
        {
            equipmentPanelContent[i].SetActive(false);
        }

        loadoutController.Initialization();
        //loadoutController.health = 50;

        //if (armorActivatePanel != null)
        //    loadoutController.protection = armorActivatePanel.value;
        
        //if (weaponActivatePanel != null)
        //    loadoutController.damage = weaponActivatePanel.value;

        switch (panelIndex)
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
