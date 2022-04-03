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

    public float value;

    public Image itemIcon;
    public Image progressBarFilled;

    public Button buyButtonComponent;
    public Button selectedWeaponButton;
    public Image buyButtonImage;

    public PanelItem panelItem;

    public Animator buyButtonAnimator;
    public Animator iconAnimator;
    public Animator buysCountTextAnimator;
    public Animator selectedButtonAnimator;

    public GameObject currentObject;

    public void Initialization(PanelItem newPanelItem)
    {
        buyButtonAnimator = buyButtonComponent.GetComponent<Animator>();
        iconAnimator = itemIcon.GetComponent<Animator>();
        buysCountTextAnimator = buysCountText.GetComponent<Animator>();
        selectedButtonAnimator = selectedWeaponButton.GetComponent<Animator>();

        buyButtonComponent.gameObject.SetActive(true);
        selectedWeaponButton.gameObject.SetActive(false);

        panelItem = newPanelItem;
        //
        value = panelItem.currentItemEquipment.value;


        if (newPanelItem.currentObject != null)
            currentObject = newPanelItem.currentObject;

        itemNameText.text = newPanelItem.itemName;
        itemIcon.sprite = newPanelItem.weaponSprite.sprite;

        //generalIncreaseValueText.text = "+$" + FormatNumsHelper.FormatNum((double)panelItem.generalIncreaseValue) + "/s";
        priceText.text = "$" + FormatNumsHelper.FormatNum((double)panelItem.price);

        buysCountText.text = panelItem.buysCount.ToString() + "/" + panelItem.costCurve.keys[panelItem.costCurve.length - 1].time.ToString();
        progressBarFilled.fillAmount = ((float)panelItem.buysCount / (float)panelItem.costCurve.keys[panelItem.costCurve.length - 1].time);

        buyButtonComponent.onClick.AddListener(() => /*newPanelItem.BuyItem*/BuyItemInOriginal());
    }

    private void FixedUpdate()
    {
        if (panelItem.price <= MoneyHandler.Instance.moneyCount)
        {
            buyButtonComponent.enabled = true;
            buyButtonImage.color = panelItem.buttonDefaultColor;
        }
        else
        {
            buyButtonComponent.enabled = false;
            buyButtonImage.color = Color.gray;
        }
    }

    public void BuyItemInOriginal()
    {
        panelItem.BuyItem();
        PlayJumpAnimation();

        if (SFX.Instance != null)
            SFX.Instance.PlayBuy();
    }

    private void PlayJumpAnimation()
    {
        buyButtonAnimator.Play("Jump", 0, 0);
        iconAnimator.Play("Jump", 0, 0);
        buysCountTextAnimator.Play("Jump", 0, 0);
    }

    public void CollapseItem()
    {
        Debug.Log("Collapse " + gameObject);
        //buyButtonComponent.onClick.RemoveListener(() => BuyItemInOriginal());
        buyButtonComponent.interactable = false;

        progressBarFilled.transform.parent.gameObject.SetActive(false);

        buyButtonComponent.gameObject.SetActive(false);
        selectedWeaponButton.gameObject.SetActive(true);
        //buyButtonComponent.onClick.AddListener(() => SelectedWeapon());
    }

    public void SelectedWeapon()
    {
        if (DungeonHubManager.dungeonHubManager == null && currentObject != null)
            return;

        selectedButtonAnimator.Play("Jump", 0, 0);

        Debug.Log("Selected");
        switch (panelItem.currentEquipmentType)
        {
            case ItemEquipment.EquipmentType.Weapon:
                if (DungeonHubManager.dungeonHubManager.weaponActivatePanel != null)
                    DungeonHubManager.dungeonHubManager.weaponActivatePanel.DeselectedWeapon();

                DungeonHubManager.dungeonHubManager.weaponActivatePanel = this;
                if (panelItem.iconSprite != null)
                    DungeonHubManager.dungeonHubManager.weaponSprite = panelItem.iconSprite;
                DungeonHubManager.dungeonHubManager.UpdateUI();

                LoadoutController.loadoutController.FillTheBar(LoadoutController.loadoutController.damageFill, value);
                break;

            case ItemEquipment.EquipmentType.Armor:
                if (DungeonHubManager.dungeonHubManager.armorActivatePanel != null)
                    DungeonHubManager.dungeonHubManager.armorActivatePanel.DeselectedWeapon();

                DungeonHubManager.dungeonHubManager.armorActivatePanel = this;
                if (panelItem.iconSprite != null)
                    DungeonHubManager.dungeonHubManager.armorSprite = panelItem.iconSprite;
                DungeonHubManager.dungeonHubManager.UpdateUI();

                LoadoutController.loadoutController.FillTheBar(LoadoutController.loadoutController.protectionFill, value);
                break;

        }

        selectedWeaponButton.interactable = false;
        currentObject.SetActive(true);

        //DungeonBuilder.Instance.CameraFocus(currentObject.transform);
    }
    public void DeselectedWeapon()
    {
        if (DungeonHubManager.dungeonHubManager == null && currentObject != null)
            return;

        selectedWeaponButton.interactable = true;

        switch (panelItem.currentEquipmentType)
        {
            case ItemEquipment.EquipmentType.Weapon:
                foreach (var currentObj in DungeonHubManager.dungeonHubManager.weaponList)
                {
                    currentObj.SetActive(false);
                    //if (currentObj == currentObject)
                    //{
                    //    DungeonHubManager.dungeonHubManager.weaponActivatePanel.DeselectedWeapon();
                    //    currentObj.SetActive(false);

                    //    return;
                    //}
                }

                break;

            case ItemEquipment.EquipmentType.Armor:
                foreach (var currentObj in DungeonHubManager.dungeonHubManager.armorList)
                {
                    currentObj.SetActive(false);
                    //if (currentObj == currentObject)
                    //{
                    //    DungeonHubManager.dungeonHubManager.aromorActivatePanel.DeselectedWeapon();
                    //    currentObj.SetActive(false);

                    //    return;
                    //}
                }
                break;
        }
    }

    public void UpdateUI()
    {
        //generalIncreaseValueText.text = "+$" + FormatNumsHelper.FormatNum((double)panelItem.generalIncreaseValue) + "/s";
        priceText.text = "$" + FormatNumsHelper.FormatNum((double)panelItem.price);

        buysCountText.text = panelItem.buysCount.ToString() + "/" + panelItem.costCurve.keys[panelItem.costCurve.length - 1].time.ToString();
        progressBarFilled.fillAmount = ((float)panelItem.buysCount / (float)panelItem.costCurve.keys[panelItem.costCurve.length - 1].time);
    }
}
