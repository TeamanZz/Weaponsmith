using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager equipmentManager;
    public List<GameObject> weaponList = new List<GameObject>();

    public int weaponNumber;
    public void Awake()
    {
        // equipmentManager = this;
        // weaponNumber = PlayerPrefs.GetInt("WeaponNumber");

        // foreach(GameObject wepon in weaponList)
        // {
        //     wepon.SetActive(false);
        // }

        // weaponList[weaponNumber].SetActive(true);
    }

    public void SaveWeaponNumber()
    {
        PlayerPrefs.SetInt("WeaponNumber", weaponNumber);
    }
    public void ShowWeaponsByNumber(int number)
    {

    }
}
