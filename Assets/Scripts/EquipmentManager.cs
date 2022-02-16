using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager equipmentManager;
    public List<GameObject> weaponList = new List<GameObject>();
    public List<GameObject> weaponOnAnwil = new List<GameObject>();

    public int weaponNumber;
    public void Awake()
    {
        equipmentManager = this;
        weaponNumber = PlayerPrefs.GetInt("WeaponNumber");
    }

    [ContextMenu("Start")]
    public void Start()
    {
        if (weaponList.Count < 0)
            return;

        weaponList[weaponNumber].gameObject.SetActive(true);
        weaponOnAnwil[weaponNumber].gameObject.SetActive(true);

        foreach (GameObject wepon in weaponList)
        {
            wepon.SetActive(false);
        }

        foreach (GameObject weaponOn in weaponOnAnwil)
        {
            weaponOn.SetActive(false);
        }

        weaponList[weaponNumber].gameObject.SetActive(true);
        weaponOnAnwil[weaponNumber].gameObject.SetActive(true);

        //Debug.Log(weaponNumber);
        //Debug.Log(weaponList[weaponNumber].gameObject + " -> First object" + " | " + weaponOnAnwil[weaponNumber] + " -> Two object");
        SaveWeaponNumber();
    }
    public void SaveWeaponNumber()
    {
        PlayerPrefs.SetInt("WeaponNumber", weaponNumber);
    }

    public void ShowWeaponsByNumber()
    {
        weaponNumber += 1;

        if(weaponNumber < 0 || weaponNumber >= weaponList.Count)
        {
            Debug.Log("Show weapon number incorrect");
            return;
        }


        foreach (GameObject wepon in weaponList)
        {
            wepon.SetActive(false);
        }

        foreach (GameObject weaponOn in weaponOnAnwil)
        {
            weaponOn.SetActive(false);
        }

        weaponList[weaponNumber].gameObject.SetActive(true);
        weaponOnAnwil[weaponNumber].gameObject.SetActive(true);

        Debug.Log(weaponNumber);
        Debug.Log(weaponList[weaponNumber].gameObject + " -> First object" + " | " + weaponOnAnwil[weaponNumber] + " -> Two object");
        SaveWeaponNumber();
    }
}
