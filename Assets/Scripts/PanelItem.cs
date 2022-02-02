using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelItem : MonoBehaviour, IBuyableItem
{
    public PanelItemState currentState;
    public enum CurrentPanel
    {
        parent,
        childObject
    }
    public CurrentPanel currentPanelState;
    public PanelItem connectPanel;

    public PanelItem nextUpgradeItem;

    public int currentWeaponNumber;

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

    public string itemName;
    [SerializeField] private int generalIncreaseValue;
    [SerializeField] private int increaseValue;
    [SerializeField] private int price;
    [SerializeField] private int buysEdgeCount;
    [SerializeField] private List<ItemCondition> itemConditionsList = new List<ItemCondition>();

    public AnimationCurve costCurve;
    public Color buttonDefaultColor;
    public Image weaponSprite;

    private void Awake()
    {
        // PlayerPrefs.DeleteAll();
        weaponSprite = itemIcon.GetComponent<Image>();
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
    }

    //show weapon
    public void ShowWeaponsByNumber()
    {
        if(currentPanelState == CurrentPanel.parent)
            EquipmentManager.equipmentManager.ShowWeaponsByNumber(currentWeaponNumber);
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
        //if (itemName == "Wooden sword")
        //    SetWaitingForDrawingItemView();
    }

    private void PlayJumpAnimation()
    {
        buyButtonAnimator.Play("Jump", 0, 0);
        iconAnimator.Play("Jump", 0, 0);
        generalIncreaseValueTextAnimator.Play("Jump", 0, 0);
        buysCountTextAnimator.Play("Jump", 0, 0);
    }

    //public void CheckConditions(PanelItem checkedItem)
    //{
    //    var findedCondition = itemConditionsList.Find(x => x.itemName == checkedItem.itemName);
    //    if (findedCondition != null)
    //    {
    //        if (findedCondition.count <= checkedItem.buysCount)
    //        {
    //            int conditionIndex = itemConditionsList.IndexOf(findedCondition);
    //            findedCondition.conditionIsMet = true;
    //            itemConditionsGO.transform.GetChild(conditionIndex).GetComponent<TextMeshProUGUI>().color = Color.green;
                
    //            //CheckAllConditionsIsMet();
    //        }
    //    }
    //}

    //private void CheckAllConditionsIsMet()
    //{
    //    int metedConditions = 0;
    //    for (int i = 0; i < itemConditionsList.Count; i++)
    //    {
    //        if (itemConditionsList[i].conditionIsMet)
    //            metedConditions++;
    //    }
    //    if (metedConditions == itemConditionsList.Count)
    //    {
    //        itemConditionsList.Clear();
    //        ItemsManager.Instance.MakeNextUnknownItemAsUnavailable();
    //        ChangeState(PanelItemState.Available);
    //    }
    //}

    private void SetItemName()
    {
        itemNameText.text = itemName;
    }

    //  тут
    public void BuyItem()
    {
        MoneyHandler.Instance.moneyCount -= price;
        MoneyHandler.Instance.IncreaseMoneyPerSecondValue(increaseValue);
        UpdateItemValues();
        UpdateView();
        PlayJumpAnimation();
        //nen
        //ItemsManager.Instance.CheckConditions(this);
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

            if(connectPanel.currentState == PanelItemState.Collapsed)
            {
                if(nextUpgradeItem == null)
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

        //WaitingForDrawing
        if (currentState == PanelItemState.WaitingForDrawing)
        {
            SetWaitingForDrawingItemView();
            PlayerPrefs.SetString("UpgradeItem" + index, "WaitingForDrawing");
        }

        if (currentState == PanelItemState.Available)
        {
            SetAvailableItemView();
            PlayerPrefs.SetString("UpgradeItem" + index, "available");
        }
    }

    //  unkown
    [ContextMenu("UnkownPanel")]
    public void UnkownPanel()
    {
        SetUnknownItemView();
        PlayerPrefs.SetString("UpgradeItem" + index, "unknown");
    }

    private void OnDestroy()
    {
        ChangeState(currentState);
        // SaveData();
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

        //WaitingForDrawing
        if (currentState == PanelItemState.WaitingForDrawing)
        {
            PlayerPrefs.SetString("UpgradeItem" + index, "WaitingForDrawing");
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
        price = (int)PlayerPrefs.GetFloat($"UpgradeItem{index}price", (int)costCurve.Evaluate(buysCount));
        increaseValue = (int)PlayerPrefs.GetFloat($"UpgradeItem{index}increaseValue", increaseValue);
        buysCount = (int)PlayerPrefs.GetFloat($"UpgradeItem{index}buysCount");
        currentState = newState;
        //ItemsManager.Instance.CheckConditions(this);

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

        if (currentState == PanelItemState.Available)
        {
            SetAvailableItemView();
            PlayerPrefs.SetString("UpgradeItem" + index, "available");
        }

        //WaitingForDrawing
        if (currentState == PanelItemState.WaitingForDrawing)
        {
            SetWaitingForDrawingItemView();
            PlayerPrefs.SetString("UpgradeItem" + index, "WaitingForDrawing");
        }

        UpdateView();
    }

    //open panel
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

        if (currentPanelState == CurrentPanel.parent)
        {
            if (connectPanel != null)
                connectPanel.ChangeStateViaLoader(PanelItemState.Available);
            else
                Debug.Log("Child panel = null " + connectPanel);
        }



        for (int i = 0; i < itemConditionsList.Count; i++)
        {
            //200 Fire power
            itemConditionsGO.transform.GetChild(i).gameObject.SetActive(true);
            string conditionText = $"{itemConditionsList[i].count} {itemConditionsList[i].itemName}";
            itemConditionsGO.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = conditionText;
        }
    }

    //WaitingForDrawing
    public void SetWaitingForDrawingItemView()
    {
        ItemsManager.Instance.currentWaitingPanel = this;

        //itemConditionsGO.SetActive(true);
        itemConditionsGO.SetActive(false);

        blurPanel.SetActive(true);
        generalIncreaseValueText.gameObject.SetActive(false);
        unknownSign.SetActive(false);
        progressBar.SetActive(false);
        buyButton.SetActive(false);
        completedSign.SetActive(false);
        itemNameText.text = "Waiting For Drawing";

        itemIcon.SetActive(false);
        unknownSign.SetActive(true);
        
        if (ItemsManager.Instance != null && ItemsManager.Instance.awardPanel != null)
        {
            ItemsManager.Instance.awardPanel.SetActive(true);
            PlayerPrefs.SetInt("AwardPanel", 1);
        }

        PlayerPrefs.SetInt("currentWaitingPanelNumber", index);

        //for (int i = 0; i < itemConditionsList.Count; i++)
        //{
        //    //200 Fire power
        //    itemConditionsGO.transform.GetChild(i).gameObject.SetActive(true);
        //    string conditionText = $"{itemConditionsList[i].count} {itemConditionsList[i].itemName}";
        //    itemConditionsGO.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = conditionText;
        //}
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

        if (currentPanelState == CurrentPanel.childObject)
        {

        }
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