using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftPanelItemsManager : MonoBehaviour
{
    public static CraftPanelItemsManager Instance;

    public GameObject newWeaponNotificationPanel;
    public List<GameObject> awardPanel = new List<GameObject>();

    [HideInInspector] public List<PanelItem> craftPanelItemsList = new List<PanelItem>();
    public PanelItem currentWaitingPanel;

    private WeaponUnlockPanel weaponPanel;
    private int awardPanelState = 0;

    [ContextMenu("Awake")]
    public void Awake()
    {
        Instance = this;
        weaponPanel = newWeaponNotificationPanel.GetComponent<WeaponUnlockPanel>();
        newWeaponNotificationPanel.SetActive(false);
        currentWaitingPanel = craftPanelItemsList[PlayerPrefs.GetInt("currentWaitingPanelNumber")];
        awardPanelState = PlayerPrefs.GetInt("AwardPanel");
        if (awardPanelState == 1)
            awardPanel[EraController.Instance.currentEraNumber].SetActive(true);
        else
            awardPanel[EraController.Instance.currentEraNumber].SetActive(false);
    }

    public void Initialization(List<PanelItem> newPanelItems)
    {
        craftPanelItemsList.Clear();
        craftPanelItemsList.AddRange(newPanelItems);

        currentWaitingPanel = craftPanelItemsList[0]; 
    }

    public void MakeNextUnknownItemAsUnavailable()
    {
        //WaitingForDrawing
        var nextItem = craftPanelItemsList.Find(x => x.currentState == PanelItemState.Unknown);
        if (nextItem != null)
            nextItem.ChangeState(PanelItemState.WaitingForDrawing);
    }

    [ContextMenu("Test")]
    public void OpenWaitingPanel()
    {
        if (currentWaitingPanel != null)
            currentWaitingPanel.ChangeState(PanelItemState.Available);

        if (newWeaponNotificationPanel != null && currentWaitingPanel != null)
        {
            newWeaponNotificationPanel.SetActive(true);
            Debug.Log(currentWaitingPanel.itemName);
            weaponPanel.InitializePanel(currentWaitingPanel.weaponSprite, currentWaitingPanel.itemName);

            EquipmentManager.Singleton.ShowWeaponsByNumber();
            awardPanel[EraController.Instance.currentEraNumber].SetActive(false);
            // PlayerPrefs.SetInt("AwardPanel", 0);
        }
    }

    public void ClosedPanel()
    {
        newWeaponNotificationPanel.SetActive(false);
    }
}