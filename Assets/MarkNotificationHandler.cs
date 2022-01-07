using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkNotificationHandler : MonoBehaviour
{
    public static MarkNotificationHandler Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject workshopMark;
    public GameObject blueprintsMark;

    public GameObject workshopPanel;
    public GameObject blueprintsPanel;

    private void Start()
    {
        InvokeRepeating("CheckRoomObjects", 1, 1);
        InvokeRepeating("CheckBlueprintsObjects", 1, 1);
    }

    private void CheckRoomObjects()
    {
        for (int i = 0; i < RoomObjectsHandler.Instance.workshopPanelItems.Count; i++)
        {
            var item = RoomObjectsHandler.Instance.workshopPanelItems[i];
            if (item.price <= MoneyHandler.Instance.moneyCount && item.wasBoughted == false && workshopPanel.activeSelf == false)
            {
                workshopMark.SetActive(true);
            }
        }
    }

    private void CheckBlueprintsObjects()
    {
        for (int i = 0; i < PrefsHandler.Instance.blueprintsList.Count; i++)
        {
            var item = PrefsHandler.Instance.blueprintsList[i];
            if (item.price <= MoneyHandler.Instance.moneyCount && item.wasBoughted == false && blueprintsPanel.activeSelf == false)
            {
                blueprintsMark.SetActive(true);
            }
        }
    }
}
