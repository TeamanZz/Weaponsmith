using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class PanelItem : MonoBehaviour, IBuyableItem
{
    public int panelID;
    public PanelItemState currentState;
    public ItemEquipment.EquipmentType currentEquipmentType;
    public string itemName;
    public int price;
    public int buysCount;
    public AnimationCurve costCurve;
    public Sprite iconSprite;
    public int currentCoefficientValue = 1;
    [SerializeField] private int maxCount = 1;
    [SerializeField] private int checkValue = 0;
    public ItemEquipment currentItemEquipment;
    public List<GameObject> currentObject = new List<GameObject>();

    [HideInInspector] public PanelItemInHub currentPanelItemInHub;
    public bool isSelected;

    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI itemNameText;
    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI priceText;
    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI coefficientText;
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
    [FoldoutGroup("View Components")][SerializeField] private Animator buysCountTextAnimator;

    [FoldoutGroup("View Components")] [SerializeField] private TextMeshProUGUI generalIncreaseValueText;
    [FoldoutGroup("View Components")] [SerializeField] private Animator generalIncreaseValueTextAnimator;
    [SerializeField] private int increaseValue;
    [FoldoutGroup("View Components")] public Color buttonDefaultColor;
    [FoldoutGroup("View Components")] public Image weaponSprite;
    private void Awake()
    {
        weaponSprite = itemIcon.GetComponent<Image>();
        generalIncreaseValueText.text = "";
        //Initialize();
        LoadData();
    }

    bool IntToBool(int n)
       => n == 1;
    int BoolToInt(bool b)
        => (b ? 1 : 0);

    public void CheckPriceCoefficient()
    {
        checkValue = 0;
        int currentPrice = (int)costCurve.Evaluate(buysCount);

        if (buysCount + CoefficientManager.coefficientValue > costCurve.keys[costCurve.length - 1].time || CoefficientManager.coefficientValue == 1)
        {
            if (CoefficientManager.coefficientValue == 1)
                maxCount = 0;
            else
                maxCount = (int)costCurve.keys[costCurve.length - 1].time; 
        }
        else
        {
            maxCount = buysCount + CoefficientManager.coefficientValue;
        }

        for (int i = buysCount; i < maxCount; i++)
        {
            int newPrice = (int)costCurve.Evaluate(i);

            if (currentPrice + newPrice <= MoneyHandler.Instance.moneyCount)
            {
                currentPrice += newPrice;
                checkValue += 1;
                MoneyHandler.Instance.SaveData();
            }
            else
            {
                break;
            }

        }

        checkValue = Mathf.Clamp(checkValue, 1, 9999);
        currentCoefficientValue = checkValue;

        price = currentPrice;
        priceText.text = "$" + FormatNumsHelper.FormatNum((double)price);
        if (coefficientText != null)
            coefficientText.text = "x" + currentCoefficientValue.ToString() + "lvl";
    }

    private void FixedUpdate()
    {
        if (currentState != PanelItemState.Available)
            return;

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
    }

    private void Start()
    {
        InvokeRepeating("SaveData", 3, 3);
        if (buysCount >= costCurve.keys[costCurve.length - 1].time)
            ChangeState(PanelItemState.Collapsed);

        ChangeState(currentState);
    }

    [ContextMenu("Save Data")]
    public void SaveData()
    {
        PlayerPrefs.SetInt($"PanelItem{panelID}State", (int)currentState);
        PlayerPrefs.SetInt($"PanelItem{panelID}Count", buysCount);
        PlayerPrefs.SetInt($"PanelItem{panelID}Selected", BoolToInt(isSelected));

        //Debug.Log("Save Data = " + panelID + " State =" + currentState + " Count =" + buysCount);
    }

    public void LoadData()
    {
        currentState = (PanelItemState)PlayerPrefs.GetInt($"PanelItem{panelID}State");
        buysCount = PlayerPrefs.GetInt($"PanelItem{panelID}Count");
        isSelected = IntToBool(PlayerPrefs.GetInt($"PanelItem{panelID}Selected"));

        //Debug.Log("Load Data = " + panelID + " State =" + currentState + " Count =" + buysCount);
        Initialize();

        if (isSelected && currentPanelItemInHub)
            currentPanelItemInHub.SelectedWeapon();
    }

    [ContextMenu("Remove Data")]
    public void RemoveData()
    {
        if (currentPanelItemInHub != null)
            Destroy(currentPanelItemInHub.gameObject);

        if (panelID == 0 /*|| panelID == 1*/)
        {
            PlayerPrefs.SetInt($"PanelItem{panelID}State", (int)PanelItemState.Available);
            ChangeState(PanelItemState.Available);
            SetAvailableItemView();
        }
        else
        {
            PlayerPrefs.SetInt($"PanelItem{panelID}State", (int)PanelItemState.Unknown);
            ChangeState(PanelItemState.Unknown);
            SetUnknownItemView();
        }

        PlayerPrefs.SetInt($"PanelItem{panelID}Count", 0);
        PlayerPrefs.SetInt($"PanelItem{panelID}Selected", BoolToInt(false));

        //Debug.Log("Remove Data = " + panelID + " State =" + currentState + " Count =" + buysCount);
        //Debug.Log("Receve Data = " + panelID);

        progressBarFilled.fillAmount = 0;
        //
        Awake();
    }

    private void Initialize()
    {
        buyButtonComponent = buyButton.GetComponent<Button>();
        buyButtonImage = buyButton.GetComponent<Image>();
        buyButtonAnimator = buyButton.GetComponent<Animator>();
        iconAnimator = itemIcon.GetComponent<Animator>();
        generalIncreaseValueTextAnimator = generalIncreaseValueText.GetComponent<Animator>();
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
        generalIncreaseValueTextAnimator.Play("Jump", 0, 0);
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

        Debug.Log("Start Save = " + panelID);
        SaveData();
        MoneyHandler.Instance.SaveData();

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
                    //Debug.Log("Collapse in parent");
                    currentPanelItemInHub.CollapseItem();
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


                if (DungeonHubManager.dungeonHubManager == null)
                    break;

                  if (currentPanelItemInHub == null)
                DungeonHubManager.dungeonHubManager.AddEquipment(this);

                generalIncreaseValueText.text = "+$" + FormatNumsHelper.FormatNum((double)increaseValue) + "/s";
                break;
        }
    }

    //  unkown
    [ContextMenu("UnkownPanel")]
    public void UnkownPanel()
    {
        SetUnknownItemView();
    }

    public void ChangeStateViaLoader(PanelItemState newState)
    {
        Initialize();
        currentState = newState;
        if (newState == PanelItemState.Collapsed)
            Debug.Log(gameObject);

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
        generalIncreaseValueText.text = "+$" + FormatNumsHelper.FormatNum((double)increaseValue) + "/s";
        itemConditionsGO.SetActive(false);
        blurPanel.SetActive(false);
        //generalIncreaseValueText.gameObject.SetActive(true);
        unknownSign.SetActive(false);
        progressBar.SetActive(true);
        buyButton.SetActive(true);
        completedSign.SetActive(false);
        itemIcon.SetActive(true);
        itemNameText.text = itemName;
        GetComponent<RectTransform>().sizeDelta = new Vector2(706, 120);
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

        MoneyHandler.Instance.IncreaseMoneyPerSecondValue(increaseValue);
        generalIncreaseValueText.text = "";

        GetComponent<RectTransform>().sizeDelta = new Vector2(680, 76);
    }

    //WaitingForDrawing
    public void SetWaitingForDrawingItemView()
    {
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

    private void UpdateItemValues()
    {
        buysCount += currentCoefficientValue;
        price = (int)costCurve.Evaluate(buysCount);
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