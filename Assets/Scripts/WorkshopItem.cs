using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorkshopItem : MonoBehaviour, IBuyableItem
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject buyButton;

    [SerializeField] private int price;

    private void Awake()
    {
        priceText.text = "$" + price.ToString();
    }

    public void BuyItem()
    {
        CollapseItemView();
    }

    public void CollapseItemView()
    {
        buyButton.SetActive(false);
        GetComponent<RectTransform>().sizeDelta = new Vector2(680, 76);
    }
}
