using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartsItem : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject buyButton;

    [SerializeField] private int price;

    private void Awake()
    {
        priceText.text = "$" + FormatNumsHelper.FormatNum((float)price);
    }

    public void BuyItem()
    {
        CraftManager.Instance.UnlockCraftWeapon(index);
        CollapseItemView();
    }

    public void CollapseItemView()
    {
        buyButton.SetActive(false);
        GetComponent<RectTransform>().sizeDelta = new Vector2(680, 76);
    }
}
