using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftItem : MonoBehaviour
{
    public ItemType itemType;
    public CraftItemPiece itemPiece;

    public void SpawnItem()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-2.4f, 1.6f), 4.3f, -1.188f);
        var newItemPiece = CraftManager.Instance.GetCraftItemPiece(itemType, itemPiece);
        Instantiate(newItemPiece, spawnPosition, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Quaternion.identity.z));
    }
}

public enum ItemType
{
    Body,
    Tip
}

public enum CraftItemPiece
{
    WoodenSword,
    GreatSwordHook,
    BlackSword,
    Axe,
    ElvenAxe,
    GreatAxe,
    GoblinMace,
    ElvenMace
}