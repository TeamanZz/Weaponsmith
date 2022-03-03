using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class BoostersItemPanel : MonoBehaviour
{
    public PanelItemState currentState;
    public BoostersItemPanel nextUpgradeItem;
    public string itemName;
    public int index;
    [SerializeField] private int increaseValue;
    [SerializeField] private int buysEdgeCount;
    public AnimationCurve costCurve;

    private int price;
    [SerializeField] private Color buttonDefaultColor;

    [FoldoutGroup("Current Runtime Values")] public int buysCount;
    [FoldoutGroup("Current Runtime Values")][SerializeField] private int generalIncreaseValue;
    [FoldoutGroup("View Components")][SerializeField] private Button buyButtonComponent;
    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI itemNameText;
    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI generalIncreaseValueText;
    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI priceText;
    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI buysCountText;
    [FoldoutGroup("View Components")][SerializeField] private GameObject buyButton;
    [FoldoutGroup("View Components")][SerializeField] private Image buyButtonImage;
    [FoldoutGroup("View Components")][SerializeField] private GameObject progressBar;
    [FoldoutGroup("View Components")][SerializeField] private GameObject completedSign;
    [FoldoutGroup("View Components")][SerializeField] private GameObject itemIcon;
    [FoldoutGroup("View Components")][SerializeField] private GameObject unknownSign;
    [FoldoutGroup("View Components")][SerializeField] private GameObject blurPanel;
    [FoldoutGroup("View Components")][SerializeField] private GameObject itemConditionsGO;
    [FoldoutGroup("View Components")][SerializeField] private Image progressBarFilled;
    [FoldoutGroup("View Components")][SerializeField] private Animator buyButtonAnimator;
    [FoldoutGroup("View Components")][SerializeField] private Animator iconAnimator;
    [FoldoutGroup("View Components")][SerializeField] private Animator generalIncreaseValueTextAnimator;
    [FoldoutGroup("View Components")][SerializeField] private Animator buysCountTextAnimator;

    private void Awake()
    {
        Initialize();
    }
    private void FixedUpdate()
    {
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

        if (buysCount >= buysEdgeCount)
            ChangeState(PanelItemState.Collapsed);
    }


    private void SaveData()
    {
        SaveState();
        PlayerPrefs.SetFloat($"BoostersUpgradeItem{index}generalIncreaseValue", generalIncreaseValue);
        PlayerPrefs.SetFloat($"BoostersUpgradeItem{index}price", price);
        PlayerPrefs.SetFloat($"BoostersUpgradeItem{index}increaseValue", increaseValue);
        PlayerPrefs.SetFloat($"BoostersUpgradeItem{index}buysCount", buysCount);
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

    //  ���
    public void BuyItem()
    {
        MoneyHandler.Instance.moneyCount -= price;
        MoneyHandler.Instance.IncreaseMoneyPerSecondValue(increaseValue);
        UpdateItemValues();
        UpdateView();
        PlayJumpAnimation();
        CheckOnCollapse();

        SaveData();
    }


    private void CheckOnCollapse()
    {
        if (buysCount >= buysEdgeCount)
        {
            ChangeState(PanelItemState.Collapsed);

            if (nextUpgradeItem == null)
            {
                Debug.Log("Next upgrade is null");
                return;
            }
            else
                nextUpgradeItem.ChangeState(PanelItemState.Available);

        }
    }

    public void ChangeState(PanelItemState newState)
    {
        currentState = newState;

        if (currentState == PanelItemState.Collapsed)
        {
            CollapseItemView();
            PlayerPrefs.SetString("BoostersUpgradeItem" + index, "collapsed");
        }

        if (currentState == PanelItemState.Unknown)
        {
            SetUnknownItemView();
            PlayerPrefs.SetString("BoostersUpgradeItem" + index, "unknown");
        }

        if (currentState == PanelItemState.Available)
        {
            SetAvailableItemView();
            PlayerPrefs.SetString("BoostersUpgradeItem" + index, "available");
        }
    }

    //  unkown
    [ContextMenu("UnkownPanel")]
    public void UnkownPanel()
    {
        SetUnknownItemView();
        PlayerPrefs.SetString("BoostersUpgradeItem" + index, "unknown");
    }

    private void SaveState()
    {
        if (currentState == PanelItemState.Collapsed)
        {
            PlayerPrefs.SetString("BoostersUpgradeItem" + index, "collapsed");
        }

        if (currentState == PanelItemState.Unknown)
        {
            PlayerPrefs.SetString("BoostersUpgradeItem" + index, "unknown");
        }

        if (currentState == PanelItemState.Available)
        {
            PlayerPrefs.SetString("BoostersUpgradeItem" + index, "available");
        }
    }

    public void ChangeStateViaLoader(PanelItemState newState)
    {
        Initialize();
        generalIncreaseValue = (int)PlayerPrefs.GetFloat($"BoostersUpgradeItem{index}generalIncreaseValue");
        price = (int)PlayerPrefs.GetFloat($"BoostersUpgradeItem{index}price", (int)costCurve.Evaluate(buysCount));
        increaseValue = (int)PlayerPrefs.GetFloat($"BoostersUpgradeItem{index}increaseValue", increaseValue);
        buysCount = (int)PlayerPrefs.GetFloat($"BoostersUpgradeItem{index}buysCount");
        currentState = newState;
        //ItemsManager.Instance.CheckConditions(this);

        if (currentState == PanelItemState.Collapsed)
        {
            CollapseItemView();
            PlayerPrefs.SetString("BoostersUpgradeItem" + index, "collapsed");
        }

        if (currentState == PanelItemState.Unknown)
        {
            SetUnknownItemView();
            PlayerPrefs.SetString("BoostersUpgradeItem" + index, "unknown");
        }

        if (currentState == PanelItemState.Available)
        {
            SetAvailableItemView();
            PlayerPrefs.SetString("BoostersUpgradeItem" + index, "available");
        }

        UpdateView();
    }

    //open panel
    [ContextMenu("Available state")]
    public void SetAvailableItemView()
    {
        itemConditionsGO.SetActive(false);
        blurPanel.SetActive(false);
        generalIncreaseValueText.gameObject.SetActive(true);
        unknownSign.SetActive(false);
        progressBar.SetActive(true);
        buyButton.SetActive(true);
        completedSign.SetActive(false);
        itemIcon.SetActive(true);
        itemNameText.text = itemName;
    }

    //Unknown
    public void SetUnknownItemView()
    {
        itemIcon.SetActive(false);
        unknownSign.SetActive(true);
        itemNameText.text = "???";
        progressBar.SetActive(false);
        generalIncreaseValueText.gameObject.SetActive(false);
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
        generalIncreaseValueText.gameObject.SetActive(false);
        buyButton.SetActive(false);
        completedSign.SetActive(true);
        itemIcon.SetActive(true);

        GetComponent<RectTransform>().sizeDelta = new Vector2(680, 76);

    }

    private void UpdateItemValues()
    {
        generalIncreaseValue += increaseValue;
        increaseValue += (2 * index);
        if (index == 0)
        {
            increaseValue = (int)(increaseValue * 1.1f);
        }
        buysCount++;
        price = (int)costCurve.Evaluate(buysCount);
        // price = (int)(price * 1.4f);
    }

    private void UpdateView()
    {
        generalIncreaseValueText.text = "+$" + FormatNumsHelper.FormatNum((double)generalIncreaseValue) + "/s";
        priceText.text = "$" + FormatNumsHelper.FormatNum((double)price);

        buysCountText.text = buysCount.ToString() + "/" + buysEdgeCount.ToString();
        progressBarFilled.fillAmount = ((float)buysCount / (float)buysEdgeCount);
    }
}
