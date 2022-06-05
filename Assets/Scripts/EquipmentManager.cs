using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    // public static EquipmentManager Singleton;

    // public List<GameObject> dungeonCharacterWeaponsList = new List<GameObject>();
    // public List<GameObject> weaponsOnAnvil = new List<GameObject>();

    // [HideInInspector] public int weaponNumber;

    // public void Awake()
    // {
    //     Singleton = this;
    //     weaponNumber = PlayerPrefs.GetInt("WeaponNumber");
    // }

    // [ContextMenu("Start")]
    // public void Start()
    // {
    //     if (dungeonCharacterWeaponsList.Count < 0)
    //         return;

    //     //dungeonCharacterWeaponsList[weaponNumber].gameObject.SetActive(true);
    //     //weaponsOnAnvil[weaponNumber].gameObject.SetActive(true);

    //     //foreach (GameObject wepon in dungeonCharacterWeaponsList)
    //     //{
    //     //    wepon.SetActive(false);
    //     //}

    //     //foreach (GameObject weaponOn in weaponsOnAnvil)
    //     //{
    //     //    weaponOn.SetActive(false);
    //     //}

    //     //dungeonCharacterWeaponsList[weaponNumber].gameObject.SetActive(true);
    //     //weaponsOnAnvil[weaponNumber].gameObject.SetActive(true);

    //     SaveWeaponNumber();
    // }
    // public void SaveWeaponNumber()
    // {
    //     // PlayerPrefs.SetInt("WeaponNumber", weaponNumber);
    // }

    // public void ShowWeaponsByNumber()
    // {
    //     weaponNumber += 1;

    //     if (weaponNumber < 0 || weaponNumber >= dungeonCharacterWeaponsList.Count)
    //     {
    //         Debug.Log("Show weapon number incorrect");
    //         return;
    //     }

    //     //foreach (GameObject wepon in dungeonCharacterWeaponsList)
    //     //{
    //     //    wepon.SetActive(false);
    //     //}

    //     //foreach (GameObject weaponOn in weaponsOnAnvil)
    //     //{
    //     //    weaponOn.SetActive(false);
    //     //}

    //     //dungeonCharacterWeaponsList[weaponNumber].gameObject.SetActive(true);
    //     //weaponsOnAnvil[weaponNumber].gameObject.SetActive(true);

    //     Debug.Log(weaponNumber);
    //     Debug.Log(dungeonCharacterWeaponsList[weaponNumber].gameObject + " -> First object" + " | " + weaponsOnAnvil[weaponNumber] + " -> Two object");
    //     SaveWeaponNumber();
    // }
}
