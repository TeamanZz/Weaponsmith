using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftItem : MonoBehaviour
{
    public ItemType itemType;
    public CraftItemPiece itemPiece;
    [SerializeField] private Animator iconAnimator;
    [SerializeField] private GameObject itemIcon;

    private void Awake()
    {
        iconAnimator = itemIcon.GetComponent<Animator>();
    }

    public void SpawnItem()
    {
        iconAnimator.Play("Jump", 0, 0);
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
    Axe,
    HookSword,
    WoodenMace,
    GoldenAxe,
    BlackSword,
    GreatAxe,
    GoldenMace,
    M1911,
    G3A4,
    SKS,
    Deagle,
    AK74,
    SVD,
    SciFiPistol,
    SciFiRifle,
    SciFiSniper
}