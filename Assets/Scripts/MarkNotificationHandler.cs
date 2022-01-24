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
   

    public GameObject workshopPanel;

    private void Start()
    {
        InvokeRepeating("CheckRoomObjects", 1, 1);
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

}
