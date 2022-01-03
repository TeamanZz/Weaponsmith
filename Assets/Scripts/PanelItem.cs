using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelItem : MonoBehaviour, IBuyableItem
{
    public PanelItemState currentState;
    public int index;
    public int buysCount;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI generalIncreaseValueText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI buysCountText;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private Button buyButtonComponent;
    [SerializeField] private Image buyButtonImage;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject completedSign;
    [SerializeField] private GameObject itemIcon;
    [SerializeField] private GameObject unknownSign;
    [SerializeField] private GameObject blurPanel;
    [SerializeField] private GameObject itemConditionsGO;
    [SerializeField] private Image progressBarFilled;

    [SerializeField] private Animator buyButtonAnimator;
    [SerializeField] private Animator iconAnimator;
    [SerializeField] private Animator generalIncreaseValueTextAnimator;
    [SerializeField] private Animator buysCountTextAnimator;

    [SerializeField] private string itemName;
    [SerializeField] private int generalIncreaseValue;
    [SerializeField] private int increaseValue;
    [SerializeField] private int price;
    [SerializeField] private int buysEdgeCount;
    [SerializeField] private List<ItemCondition> itemConditionsList = new List<ItemCondition>();

    public AnimationCurve costCurve;
    public Color buttonDefaultColor;
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

    private void Awake()
    {
        // PlayerPrefs.DeleteAll();
        Initialize();
    }

    private void Start()
    {
        InvokeRepeating("SaveData", 3, 3);
    }

    private void SaveData()
    {
        SaveState();
        PlayerPrefs.SetFloat($"UpgradeItem{index}generalIncreaseValue", generalIncreaseValue);
        PlayerPrefs.SetFloat($"UpgradeItem{index}price", price);
        PlayerPrefs.SetFloat($"UpgradeItem{index}increaseValue", increaseValue);
        PlayerPrefs.SetFloat($"UpgradeItem{index}buysCount", buysCount);
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
        if (itemName == "Hammer power")
            SetUnavailableItemView();
    }

    private void PlayJumpAnimation()
    {
        buyButtonAnimator.Play("Jump", 0, 0);
        iconAnimator.Play("Jump", 0, 0);
        generalIncreaseValueTextAnimator.Play("Jump", 0, 0);
        buysCountTextAnimator.Play("Jump", 0, 0);
    }

    public void CheckConditions(PanelItem checkedItem)
    {
        var findedCondition = itemConditionsList.Find(x => x.itemName == checkedItem.itemName);
        if (findedCondition != null)
        {
            if (findedCondition.count <= checkedItem.buysCount)
            {
                int conditionIndex = itemConditionsList.IndexOf(findedCondition);
                findedCondition.conditionIsMet = true;
                itemConditionsGO.transform.GetChild(conditionIndex).GetComponent<TextMeshProUGUI>().color = Color.green;
                CheckAllConditionsIsMet();
            }
        }
    }

    private void CheckAllConditionsIsMet()
    {
        int metedConditions = 0;
        for (int i = 0; i < itemConditionsList.Count; i++)
        {
            if (itemConditionsList[i].conditionIsMet)
                metedConditions++;
        }
        if (metedConditions == itemConditionsList.Count)
        {
            itemConditionsList.Clear();
            Debug.Log("create new" + gameObject.name);
            ItemsManager.Instance.MakeNextUnknownItemAsUnavailable();
            ChangeState(PanelItemState.Available);
        }
    }

    private void SetItemName()
    {
        itemNameText.text = itemName;
    }

    public void BuyItem()
    {
        MoneyHandler.Instance.moneyCount -= price;
        MoneyHandler.Instance.IncreaseMoneyPerSecondValue(increaseValue);
        UpdateItemValues();
        UpdateView();
        PlayJumpAnimation();
        ItemsManager.Instance.CheckConditions(this);
        CheckOnCollapse();
        // SetUnknownItemView();
        // CollapseItemView();
        // SetUnavailableItemView();
    }

    private void CheckOnCollapse()
    {
        if (buysCount >= buysEdgeCount)
        {
            ChangeState(PanelItemState.Collapsed);
        }
    }

    public void ChangeState(PanelItemState newState)
    {
        currentState = newState;

        if (currentState == PanelItemState.Collapsed)
        {
            CollapseItemView();
            PlayerPrefs.SetString("UpgradeItem" + index, "collapsed");
        }

        if (currentState == PanelItemState.Unknown)
        {
            SetUnknownItemView();
            PlayerPrefs.SetString("UpgradeItem" + index, "unknown");
        }

        if (currentState == PanelItemState.Unavailable)
        {
            SetUnavailableItemView();
            PlayerPrefs.SetString("UpgradeItem" + index, "unavailable");
        }

        if (currentState == PanelItemState.Available)
        {
            SetAvailableItemView();
            PlayerPrefs.SetString("UpgradeItem" + index, "available");
        }
    }

    private void OnDestroy()
    {
        ChangeState(currentState);
        SaveData();
    }

    private void SaveState()
    {
        if (currentState == PanelItemState.Collapsed)
        {
            PlayerPrefs.SetString("UpgradeItem" + index, "collapsed");
        }

        if (currentState == PanelItemState.Unknown)
        {
            PlayerPrefs.SetString("UpgradeItem" + index, "unknown");
        }

        if (currentState == PanelItemState.Unavailable)
        {
            PlayerPrefs.SetString("UpgradeItem" + index, "unavailable");
        }

        if (currentState == PanelItemState.Available)
        {
            PlayerPrefs.SetString("UpgradeItem" + index, "available");
        }
    }

    public void ChangeStateViaLoader(PanelItemState newState)
    {
        Initialize();
        generalIncreaseValue = (int)PlayerPrefs.GetFloat($"UpgradeItem{index}generalIncreaseValue");
        price = (int)PlayerPrefs.GetFloat($"UpgradeItem{index}price");
        increaseValue = (int)PlayerPrefs.GetFloat($"UpgradeItem{index}increaseValue");
        buysCount = (int)PlayerPrefs.GetFloat($"UpgradeItem{index}buysCount");
        currentState = newState;
        ItemsManager.Instance.CheckConditions(this);

        if (currentState == PanelItemState.Collapsed)
        {
            CollapseItemView();
            PlayerPrefs.SetString("UpgradeItem" + index, "collapsed");
        }

        if (currentState == PanelItemState.Unknown)
        {
            SetUnknownItemView();
            PlayerPrefs.SetString("UpgradeItem" + index, "unknown");
        }

        if (currentState == PanelItemState.Unavailable)
        {
            SetUnavailableItemView();
            PlayerPrefs.SetString("UpgradeItem" + index, "unavailable");
        }

        if (currentState == PanelItemState.Available)
        {
            SetAvailableItemView();
            PlayerPrefs.SetString("UpgradeItem" + index, "available");
        }
        UpdateView();
    }

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

        for (int i = 0; i < itemConditionsList.Count; i++)
        {
            //200 Fire power
            itemConditionsGO.transform.GetChild(i).gameObject.SetActive(true);
            string conditionText = $"{itemConditionsList[i].count} {itemConditionsList[i].itemName}";
            itemConditionsGO.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = conditionText;
        }
    }

    public void SetUnavailableItemView()
    {
        itemConditionsGO.SetActive(true);
        blurPanel.SetActive(true);
        generalIncreaseValueText.gameObject.SetActive(false);
        unknownSign.SetActive(false);
        progressBar.SetActive(false);
        buyButton.SetActive(false);
        completedSign.SetActive(false);
        itemNameText.text = itemName;
        itemIcon.SetActive(true);

        for (int i = 0; i < itemConditionsList.Count; i++)
        {
            //200 Fire power
            itemConditionsGO.transform.GetChild(i).gameObject.SetActive(true);
            string conditionText = $"{itemConditionsList[i].count} {itemConditionsList[i].itemName}";
            itemConditionsGO.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = conditionText;
        }
    }

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

    public void CollapseItemView()
    {
        progressBar.SetActive(false);
        generalIncreaseValueText.gameObject.SetActive(false);
        buyButton.SetActive(false);
        completedSign.SetActive(true);
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
        generalIncreaseValueText.text = "+$" + FormatNumsHelper.FormatNum((float)generalIncreaseValue) + "/s";
        priceText.text = "$" + FormatNumsHelper.FormatNum((float)price);

        buysCountText.text = buysCount.ToString() + "/" + buysEdgeCount.ToString();
        progressBarFilled.fillAmount = ((float)buysCount / (float)buysEdgeCount);
    }
}