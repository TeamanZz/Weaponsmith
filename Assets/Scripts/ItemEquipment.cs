using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemEquipment 
{
    public Sprite itemSprite;
    public int maxCount;
    public int currentCount;

    public int value;

    public PanelItem currentPanelItem;

    public EquipmentType equipmentType;
    public enum EquipmentType
    {
        Weapon,
        Armor,
        Empty
    }
}
