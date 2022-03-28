using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelItemInHub : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    //public TextMeshProUGUI generalIncreaseValueText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI buysCountText;

    public Image itemIcon;
    public Image progressBarFilled;

    public Button buyButtonComponent;
    public PanelItem panelItem;
    public void Initialization(PanelItem newPanelItem)
    {
        panelItem = newPanelItem;

        itemNameText.text = newPanelItem.itemName;
        itemIcon.sprite = newPanelItem.weaponSprite.sprite;

        //generalIncreaseValueText.text = "+$" + FormatNumsHelper.FormatNum((double)panelItem.generalIncreaseValue) + "/s";
        priceText.text = "$" + FormatNumsHelper.FormatNum((double)panelItem.price);

        buysCountText.text = panelItem.buysCount.ToString() + "/" + panelItem.costCurve.keys[panelItem.costCurve.length - 1].time.ToString();
        progressBarFilled.fillAmount = ((float)panelItem.buysCount / (float)panelItem.costCurve.keys[panelItem.costCurve.length - 1].time);

        buyButtonComponent.onClick.AddListener(() => newPanelItem.BuyItem());
    }

    public void UpdateUI()
    {
        //generalIncreaseValueText.text = "+$" + FormatNumsHelper.FormatNum((double)panelItem.generalIncreaseValue) + "/s";
        priceText.text = "$" + FormatNumsHelper.FormatNum((double)panelItem.price);

        buysCountText.text = panelItem.buysCount.ToString() + "/" + panelItem.costCurve.keys[panelItem.costCurve.length - 1].time.ToString();
        progressBarFilled.fillAmount = ((float)panelItem.buysCount / (float)panelItem.costCurve.keys[panelItem.costCurve.length - 1].time);
    }
}
