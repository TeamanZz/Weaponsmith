using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorkshopItem : MonoBehaviour, IBuyableItem
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject buyButton;

    [SerializeField] private int index;
    [SerializeField] private int price;

    private void Awake()
    {
        priceText.text = "$" + FormatNumsHelper.FormatNum((float)price);
    }

    public void BuyItem()
    {
        RoomObjectsHandler.Instance.UnlockObject(index);
        CollapseItemView();
    }

    public void CollapseItemView()
    {
        buyButton.SetActive(false);
        GetComponent<RectTransform>().sizeDelta = new Vector2(680, 76);
    }
}
