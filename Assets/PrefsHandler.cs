using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsHandler : MonoBehaviour
{
    private void Start()
    {
        LoadWorshopItems();
    }

    private void OnDisable()
    {
        SaveMoney();
    }

    private void SaveMoney()
    {
        PlayerPrefs.SetFloat("MoneyCount", MoneyHandler.Instance.moneyCount);
    }

    private void LoadWorshopItems()
    {
        for (int i = 0; i < RoomObjectsHandler.Instance.roomObjects.Count; i++)
        {
            string state = PlayerPrefs.GetString("WorkshopGameobject" + i);
            if (state == "unlocked")
            {
                RoomObjectsHandler.Instance.workshopPanelItems[i].CollapseItemView();
                RoomObjectsHandler.Instance.UnlockObjectWithoutParticles(i);
            }
        }
    }
}