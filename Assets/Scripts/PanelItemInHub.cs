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

    public int armorIndex;
    public int value;

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

    public List<GameObject> currentObject = new List<GameObject>();

    public TextMeshProUGUI viewValueText;
    public Image viewIcon;
    public Sprite[] viewSprite;


    public void Initialization(PanelItem newPanelItem)
    {
        viewValueText.gameObject.SetActive(false);

        buyButtonAnimator = buyButtonComponent.GetComponent<Animator>();
        iconAnimator = itemIcon.GetComponent<Animator>();
        buysCountTextAnimator = buysCountText.GetComponent<Animator>();
        selectedButtonAnimator = selectedWeaponButton.GetComponent<Animator>();

        buyButtonComponent.gameObject.SetActive(true);
        selectedWeaponButton.gameObject.SetActive(false);

        panelItem = newPanelItem;
        value = panelItem.currentItemEquipment.value;
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
        panelItem.currentCoefficientValue = 1;
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

        viewValueText.gameObject.SetActive(true);
        switch (panelItem.currentEquipmentType)
        {
            case ItemEquipment.EquipmentType.Weapon:
                viewValueText.text = panelItem.currentItemEquipment.value.ToString();
                viewIcon.sprite = viewSprite[0];
                break;

            case ItemEquipment.EquipmentType.Armor:
                viewValueText.text = panelItem.currentItemEquipment.value.ToString();
                viewIcon.sprite = viewSprite[1];
                break;
        }
            //buyButtonComponent.onClick.AddListener(() => SelectedWeapon());
        }

    public void SelectedWeapon()
    {
        if (DungeonHubManager.dungeonHubManager == null && currentObject != null)
            return;

        selectedButtonAnimator.Play("Jump", 0, 0);
        viewValueText.gameObject.SetActive(true);

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

                viewValueText.text = panelItem.currentItemEquipment.value.ToString();
                viewIcon.sprite = viewSprite[0];

                //LoadoutController.Instance.FillTheBar(LoadoutController.Instance.fill[2], value);
                break;

            case ItemEquipment.EquipmentType.Armor:
                if (DungeonHubManager.dungeonHubManager.armorActivatePanel != null)
                    DungeonHubManager.dungeonHubManager.armorActivatePanel.DeselectedWeapon();

                DungeonHubManager.dungeonHubManager.armorActivatePanel = this;
                if (panelItem.iconSprite != null)
                    DungeonHubManager.dungeonHubManager.armorSprite = panelItem.iconSprite;
                DungeonHubManager.dungeonHubManager.UpdateUI();

                if (DungeonHubManager.dungeonHubManager.armorList[0] != null)
                    DungeonHubManager.dungeonHubManager.armorList[0].SetActive(false);

                viewValueText.text = panelItem.currentItemEquipment.value.ToString();
                viewIcon.sprite = viewSprite[1];

                //LoadoutController.Instance.FillTheBar(LoadoutController.Instance.fill[1], value);
                // SkinsManager.Instance.ChangeSkin()
                break;

        }

        selectedWeaponButton.interactable = false;

        foreach (var currentObjectInPanel in currentObject)
        {
            if (currentObjectInPanel != null)
                currentObjectInPanel.SetActive(true);
        }
        //currentObject.SetActive(true);

        LoadoutController.Instance.Initialization();
        LoadoutController.Instance.PlaySelectedAnimation();

        //DungeonBuilder.Instance.CameraFocus(currentObject.transform);
    }
    public void DeselectedWeapon()
    {
        if (DungeonHubManager.dungeonHubManager == null && currentObject != null)
            return;

        selectedWeaponButton.interactable = true;

        foreach (var currentObjectInPanel in currentObject)
        {
            if (currentObjectInPanel != null)
                currentObjectInPanel.SetActive(false);
        }

        switch (panelItem.currentEquipmentType)
        {
            case ItemEquipment.EquipmentType.Weapon:
                foreach (var currentObj in DungeonHubManager.dungeonHubManager.weaponList)
                {
                    currentObj.SetActive(false);
                }
                break;

            case ItemEquipment.EquipmentType.Armor:
                foreach (var currentObj in DungeonHubManager.dungeonHubManager.armorList)
                {
                    currentObj.SetActive(false);
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
