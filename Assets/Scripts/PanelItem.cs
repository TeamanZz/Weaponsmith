using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelItem : MonoBehaviour, IBuyableItem
{
    public PanelItemState currentState;
    public int buysCount;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI generalIncreaseValueText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI buysCountText;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject completedSign;
    [SerializeField] private GameObject itemIcon;
    [SerializeField] private GameObject unknownSign;
    [SerializeField] private GameObject blurPanel;
    [SerializeField] private GameObject itemConditionsGO;
    [SerializeField] private Image progressBarFilled;

    [SerializeField] private string itemName;
    [SerializeField] private int generalIncreaseValue;
    [SerializeField] private int increaseValue;
    [SerializeField] private int price;
    [SerializeField] private int buysEdgeCount;
    [SerializeField] private List<ItemCondition> itemConditionsList = new List<ItemCondition>();

    public AnimationCurve costCurve;

    private void Awake()
    {
        if (currentState != PanelItemState.Unknown)
            SetItemName();
        price = (int)costCurve.Evaluate(buysCount);
        UpdateView();
        if (itemName == "Hammer power")
            SetUnavailableItemView();
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
        MoneyHandler.Instance.IncreaseMoneyPerSecondValue(increaseValue);
        UpdateItemValues();
        UpdateView();

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
        }

        if (currentState == PanelItemState.Unknown)
        {
            SetUnknownItemView();
        }

        if (currentState == PanelItemState.Unavailable)
        {
            SetUnavailableItemView();
        }

        if (currentState == PanelItemState.Available)
        {
            SetAvailableItemView();
        }
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
        increaseValue = (int)(increaseValue * 1.2f);
        buysCount++;
        price = (int)costCurve.Evaluate(buysCount);
        // price = (int)(price * 1.4f);
    }

    private void UpdateView()
    {
        generalIncreaseValueText.text = "+$" + generalIncreaseValue.ToString() + "/s";
        priceText.text = "$" + price.ToString();

        buysCountText.text = buysCount.ToString() + "/" + buysEdgeCount.ToString();
        progressBarFilled.fillAmount = ((float)buysCount / (float)buysEdgeCount);
    }
}