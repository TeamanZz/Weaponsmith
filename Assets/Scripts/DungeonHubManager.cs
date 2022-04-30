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

    [HideInInspector] public PanelItemInHub armorActivatePanel;
    [HideInInspector] public PanelItemInHub weaponActivatePanel;

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

        armorList[0].SetActive(true);
        UpdateUI();
    }

    public void UpdateUI()
    {
        armorUIImage.sprite = armorSprite;
        weaponUIImage.sprite = weaponSprite;
    }
    public void AddEquipment(PanelItem newPunelItem)
    {

        if (newPunelItem.currentEquipmentType == ItemEquipment.EquipmentType.Empty)
            return;

        PanelItemInHub newPanelItemInHub = null;

        switch (newPunelItem.currentEquipmentType)
        {
            case ItemEquipment.EquipmentType.Weapon:
                newPanelItemInHub = Instantiate(panelInHubPrefab, weaponContent);
                newPanelItemInHub.transform.SetParent(weaponContent);
                break;

            case ItemEquipment.EquipmentType.Armor:
                newPanelItemInHub = Instantiate(panelInHubPrefab, weaponContent);
                newPanelItemInHub.transform.SetParent(armorContent);
                break;
        }

        newPanelItemInHub.Initialization(newPunelItem);
        newPunelItem.currentPanelItemInHub = newPanelItemInHub;

    }
    public void OpenPanel(int panelIndex)
    {
        for (int i = 0; i < equipmentPanelContent.Count; i++)
        {
            equipmentPanelContent[i].SetActive(false);
        }

        loadoutController.Initialization();

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