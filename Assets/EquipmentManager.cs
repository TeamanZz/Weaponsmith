using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager equipmentManager;
    public List<GameObject> weaponList = new List<GameObject>();

    public void Awake()
    {
        equipmentManager = this;
    }
    public void ShowWeaponsByNumber(int number)
    {

    }
}
