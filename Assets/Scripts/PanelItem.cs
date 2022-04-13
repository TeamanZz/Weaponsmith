using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class PanelItem : MonoBehaviour, IBuyableItem
{
    public PanelItemState currentState;
    public CurrentPanel currentPanelState;
    public ItemEquipment.EquipmentType currentEquipmentType;

    public PanelItem connectPanel;
    public PanelItem nextUpgradeItem;
    public string itemName;
    public int index;
    public int currentWeaponNumber = 0;
    //[SerializeField] private int increaseValue;
    [Space]
    public int buysCount;
    [Space]
    public AnimationCurve costCurve;

    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI itemNameText;
    //[FoldoutGroup("View Components")] [SerializeField] private TextMeshProUGUI generalIncreaseValueText;
    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI priceText;
    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI buysCountText;
    [FoldoutGroup("View Components")][SerializeField] private GameObject buyButton;
    [FoldoutGroup("View Components")][FoldoutGroup("View Components")][SerializeField] private Button buyButtonComponent;
    [FoldoutGroup("View Components")][SerializeField] public Image buyButtonImage;
    [FoldoutGroup("View Components")][SerializeField] public GameObject progressBar;
    [FoldoutGroup("View Components")][SerializeField] private GameObject completedSign;
    [FoldoutGroup("View Components")][SerializeField] private GameObject itemIcon;
    [FoldoutGroup("View Components")][SerializeField] private GameObject unknownSign;
    [FoldoutGroup("View Components")][SerializeField] private GameObject blurPanel;
    [FoldoutGroup("View Components")][SerializeField] private GameObject itemConditionsGO;
    [FoldoutGroup("View Components")][SerializeField] private Image progressBarFilled;
    [FoldoutGroup("View Components")][SerializeField] private Animator buyButtonAnimator;
    [FoldoutGroup("View Components")][SerializeField] private Animator iconAnimator;
    //[FoldoutGroup("View Components")] [SerializeField] private Animator generalIncreaseValueTextAnimator;
    [FoldoutGroup("View Components")][SerializeField] private Animator buysCountTextAnimator;
    [FoldoutGroup("View Components")] public Color buttonDefaultColor;
    [FoldoutGroup("View Components")] public Image weaponSprite;
    //public int generalIncreaseValue;
    public int price;

    public ItemEquipment currentItemEquipment;
    public PanelItemInHub currentPanelItemInHub;

    public GameObject currentObject;
    [Header("Sprite")]
    public Sprite iconSprite;
    private void Awake()
    {
        weaponSprite = itemIcon.GetComponent<Image>();
        Initialize();
    }

    [SerializeField] private int currentCoefficientValue = 1;
    [SerializeField] private int maxCount = 1;
    [SerializeField] private int checkValue = 0;
    [SerializeField] private List<Vector2Int> priceList = new List<Vector2Int>();
   public void CheckPriceCoefficient()
    {
        priceList.Clear();
        checkValue = 0;
        int currentPrice = (int)costCurve.Evaluate(buysCount);
        //Debug.Log("Panel " + itemNameText.text + " - Coef " + (buysCount + CoefficientManager.coefficientValue) + " Lenght " + costCurve.keys[costCurve.length - 1].time);
        if (buysCount + CoefficientManager.coefficientValue > costCurve.keys[costCurve.length - 1].time || CoefficientManager.coefficientValue == 1)
        {
            if (CoefficientManager.coefficientValue == 1)
                maxCount = 1;
            else
                maxCount = (int)costCurve.keys[costCurve.length - 1].time - buysCount;

            priceList.Add(new Vector2Int(currentPrice, currentPrice));

            checkValue = maxCount;
        }
        else
        {
            maxCount = buysCount + CoefficientManager.coefficientValue;
        }

        //Debug.Log("Panel " + name + " MaxCount " + maxCount);

        for (int i = buysCount; i < maxCount; i++)
        {
            int newPrice = (int)costCurve.Evaluate(i);

            if (currentPrice + newPrice <= MoneyHandler.Instance.moneyCount)
            {
                currentPrice += newPrice;
                priceList.Add(new Vector2Int(currentPrice, newPrice));
                checkValue += 1;
            }
            else
            {
                break;
            }

            Debug.Log("Panel " + name + "Number " + i + " Result " + checkValue);
        }

        checkValue = Mathf.Clamp(checkValue, buysCount, buysCount + CoefficientManager.coefficientValue);
        currentCoefficientValue = checkValue;

        price = currentPrice;
        priceText.text = "$" + FormatNumsHelper.FormatNum((double)price);
    }

    private void FixedUpdate()
    {
        CheckPriceCoefficient();
        if (price <= MoneyHandler.Instance.moneyCount)
        {
            buyButtonComponent.enabled = true;
            buyButtonImage.color = buttonDefaultColor;
        }
        else
        {
            buyButtonComponent.enabled = false;
            buyButtonImage.color = Color.gray;
        }
        CheckOnCollapse();
    }

    private void Start()
    {
        InvokeRepeating("SaveData", 3, 3);
        if (buysCount >= costCurve.keys[costCurve.length - 1].time)
            ChangeState(PanelItemState.Collapsed);

        ChangeState(currentState);
    }

    //show weapon
    public void ShowWeaponsByNumber()
    {
        if (currentPanelState == CurrentPanel.parent)
        {
            Debug.Log("\n" + "Current weapon number - " + currentWeaponNumber);
        }
    }
    private void SaveData()
    {
        // SaveState();
        // PlayerPrefs.SetFloat($"UpgradeItem{index}generalIncreaseValue", generalIncreaseValue);
        // PlayerPrefs.SetFloat($"UpgradeItem{index}price", price);
        // PlayerPrefs.SetFloat($"UpgradeItem{index}increaseValue", increaseValue);
        // PlayerPrefs.SetFloat($"UpgradeItem{index}buysCount", buysCount);
    }

    private void Initialize()
    {
        buyButtonComponent = buyButton.GetComponent<Button>();
        buyButtonImage = buyButton.GetComponent<Image>();
        buyButtonAnimator = buyButton.GetComponent<Animator>();
        iconAnimator = itemIcon.GetComponent<Animator>();
        //        generalIncreaseValueTextAnimator = generalIncreaseValueText.GetComponent<Animator>();
        buysCountTextAnimator = buysCountText.GetComponent<Animator>();

        if (currentState != PanelItemState.Unknown)
            SetItemName();
        price = (int)costCurve.Evaluate(buysCount);
        UpdateView();
    }

    private void PlayJumpAnimation()
    {
        buyButtonAnimator.Play("Jump", 0, 0);
        iconAnimator.Play("Jump", 0, 0);
        //        generalIncreaseValueTextAnimator.Play("Jump", 0, 0);
        buysCountTextAnimator.Play("Jump", 0, 0);
    }

    private void SetItemName()
    {
        itemNameText.text = itemName;
    }

    public void BuyItem()
    {
        MoneyHandler.Instance.moneyCount -= price;
        //MoneyHandler.Instance.IncreaseMoneyPerSecondValue(increaseValue);
        UpdateItemValues();
        UpdateView();
        PlayJumpAnimation();
        CheckOnCollapse();

        if (currentPanelItemInHub == null)
            return;
        currentPanelItemInHub.UpdateUI();
    }

    private void CheckOnCollapse()
    {
        if (buysCount >= costCurve.keys[costCurve.length - 1].time)
        {

            SFX.Instance.PlayQuestComplete();
            ChangeState(PanelItemState.Collapsed);

            if (connectPanel.currentState == PanelItemState.Collapsed)
            {
                //if (currentPanelItemInHub != null)
                //{
                //    Debug.Log("Collapse in parent");
                //    currentPanelItemInHub.CollapseItem();
                //}

                if (nextUpgradeItem == null)
                {
                    Debug.Log("Next upgrade is null");
                    return;
                }

                nextUpgradeItem.ChangeState(PanelItemState.WaitingForDrawing);
            }
        }
    }

    public void ChangeState(PanelItemState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case PanelItemState.Collapsed:
                CollapseItemView();

                if (currentPanelItemInHub != null)
                {
                    Debug.Log("Collapse in parent");
                    currentPanelItemInHub.CollapseItem();
                    currentPanelItemInHub.SelectedWeapon();
                }
                else
                {
                    DungeonHubManager.dungeonHubManager.AddEquipment(this);
                    currentPanelItemInHub.CollapseItem();
                }

                break;

            case PanelItemState.Unknown:
                SetUnknownItemView();
                break;

            case PanelItemState.WaitingForDrawing:
                SetWaitingForDrawingItemView();
                break;

            case PanelItemState.Available:
                SetAvailableItemView();
                currentItemEquipment.currentCount = buysCount;

                if (currentPanelState == CurrentPanel.parent)
                {
                    if (DungeonHubManager.dungeonHubManager == null)
                        break;

                    DungeonHubManager.dungeonHubManager.AddEquipment(this);
                    if (connectPanel != null)
                        DungeonHubManager.dungeonHubManager.AddEquipment(connectPanel);
                    //connectPanel.ChangeState(connectPanel.currentState);
                }
                Debug.Log("Start");
                break;
        }
    }

    //  unkown
    [ContextMenu("UnkownPanel")]
    public void UnkownPanel()
    {
        SetUnknownItemView();
        // PlayerPrefs.SetString("UpgradeItem" + index, "unknown");
    }

    private void SaveState()
    {
        if (currentState == PanelItemState.Collapsed)
        {
            // PlayerPrefs.SetString("UpgradeItem" + index, "collapsed");
        }

        if (currentState == PanelItemState.Unknown)
        {
            // PlayerPrefs.SetString("UpgradeItem" + index, "unknown");
        }

        //WaitingForDrawing
        if (currentState == PanelItemState.WaitingForDrawing)
        {
            // PlayerPrefs.SetString("UpgradeItem" + index, "WaitingForDrawing");
        }

        if (currentState == PanelItemState.Available)
        {
            // PlayerPrefs.SetString("UpgradeItem" + index, "available");
        }
    }

    public void ChangeStateViaLoader(PanelItemState newState)
    {
        Initialize();
        //generalIncreaseValue = (int)PlayerPrefs.GetFloat($"UpgradeItem{index}generalIncreaseValue");
        // price = (int)PlayerPrefs.GetFloat($"UpgradeItem{index}price", (int)costCurve.Evaluate(buysCount));
        //increaseValue = (int)PlayerPrefs.GetFloat($"UpgradeItem{index}increaseValue", increaseValue);
        // buysCount = (int)PlayerPrefs.GetFloat($"UpgradeItem{index}buysCount");
        currentState = newState;
        if (newState == PanelItemState.Collapsed)
            Debug.Log(gameObject);
        //ItemsManager.Instance.CheckConditions(this);

        switch (currentState)
        {
            case PanelItemState.Collapsed:
                CollapseItemView();
                break;

            case PanelItemState.Unknown:
                SetUnknownItemView();
                break;

            case PanelItemState.WaitingForDrawing:
                SetWaitingForDrawingItemView();
                break;

            case PanelItemState.Available:
                SetAvailableItemView();
                //DungeonHubManager.dungeonHubManager.AddEquipment(this);
                break;
        }


        UpdateView();
    }

    //open panel
    public void SetAvailableItemView()
    {
        itemConditionsGO.SetActive(false);
        blurPanel.SetActive(false);
        //generalIncreaseValueText.gameObject.SetActive(true);
        unknownSign.SetActive(false);
        progressBar.SetActive(true);
        buyButton.SetActive(true);
        completedSign.SetActive(false);
        itemIcon.SetActive(true);
        itemNameText.text = itemName;

        if (currentPanelState == CurrentPanel.parent)
        {
            if (connectPanel != null)
                connectPanel.ChangeStateViaLoader(PanelItemState.Available);
            else
                Debug.Log("Child panel = null " + connectPanel);
        }
    }

    //WaitingForDrawing
    public void SetWaitingForDrawingItemView()
    {
        CraftPanelItemsManager.Instance.currentWaitingPanel = this;

        //itemConditionsGO.SetActive(true);
        itemConditionsGO.SetActive(false);

        blurPanel.SetActive(true);
        //generalIncreaseValueText.gameObject.SetActive(false);
        unknownSign.SetActive(false);
        progressBar.SetActive(false);
        buyButton.SetActive(false);
        completedSign.SetActive(false);
        itemNameText.text = "Waiting For Drawing";

        itemIcon.SetActive(false);
        unknownSign.SetActive(true);

        if (CraftPanelItemsManager.Instance != null && CraftPanelItemsManager.Instance.awardPanel != null)
        {
            CraftPanelItemsManager.Instance.awardPanel[EraController.Instance.currentEraNumber].SetActive(true);
            // PlayerPrefs.SetInt("AwardPanel", 1);
        }

        // PlayerPrefs.SetInt("currentWaitingPanelNumber", index);
    }

    //Unknown
    public void SetUnknownItemView()
    {
        itemIcon.SetActive(false);
        unknownSign.SetActive(true);
        itemNameText.text = "???";
        progressBar.SetActive(false);
        //generalIncreaseValueText.gameObject.SetActive(false);
        buyButton.SetActive(false);
        completedSign.SetActive(false);
        blurPanel.SetActive(true);
    }

    //collapse
    public void CollapseItemView()
    {
        unknownSign.SetActive(false);
        blurPanel.SetActive(false);
        itemNameText.text = itemName;
        progressBar.SetActive(false);
        //generalIncreaseValueText.gameObject.SetActive(false);
        buyButton.SetActive(false);
        completedSign.SetActive(true);
        itemIcon.SetActive(true);

        GetComponent<RectTransform>().sizeDelta = new Vector2(680, 76);
    }

    private void UpdateItemValues()
    {
        buysCount += currentCoefficientValue;
        //buysCount += 1;
        price = (int)costCurve.Evaluate(buysCount);// + PanelsHandler.Instance.numberOfPurchases);
    }

    private void UpdateView()
    {
        //generalIncreaseValueText.text = "+$" + FormatNumsHelper.FormatNum((double)generalIncreaseValue) + "/s";
        priceText.text = "$" + FormatNumsHelper.FormatNum((double)price);

        buysCountText.text = buysCount.ToString() + "/" + costCurve.keys[costCurve.length - 1].time.ToString();
        progressBarFilled.fillAmount = ((float)buysCount / (float)costCurve.keys[costCurve.length - 1].time);
    }

    public enum CurrentPanel
    {
        parent,
        childObject
    }
}