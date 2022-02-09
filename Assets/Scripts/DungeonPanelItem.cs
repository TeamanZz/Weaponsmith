using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DungeonPanelItem : MonoBehaviour
{
    public PanelItemState currentState;
    public enum CurrentPanel
    {
        parent,
        childObject
    }
    public CurrentPanel currentPanelState;

    public enum ArmorOrEnemy
    {
        none,
        armor,
        enemy
    }
    public ArmorOrEnemy currentObject;

    public DungeonPanelItem connectPanel;
    public DungeonPanelItem nextUpgradeItem;
    public int index;
    public string itemName;
    [SerializeField] private int increaseValue;
    [SerializeField] private int buysEdgeCount;
    public AnimationCurve costCurve;

    [Space]
    public int buysCount;
    [SerializeField] private int generalIncreaseValue;

    private int price;
    //[HideInInspector] public int currentWeaponNumber;
    [Space]
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI generalIncreaseValueText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI buysCountText;
    [SerializeField] private GameObject buyButton;
    private Button buyButtonComponent;
    private Image buyButtonImage;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject completedSign;
    [SerializeField] private GameObject itemIcon;
    [SerializeField] private GameObject unknownSign;
    [SerializeField] private GameObject blurPanel;
    [SerializeField] private GameObject itemConditionsGO;
    [SerializeField] private Image progressBarFilled;

    private Animator buyButtonAnimator;
    private Animator iconAnimator;
    private Animator generalIncreaseValueTextAnimator;
    private Animator buysCountTextAnimator;


    public Color buttonDefaultColor;

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

    //show weapon
    //public void ShowWeaponsByNumber()
    //{
    // //   if (currentPanelState == CurrentPanel.parent)
    //        //EquipmentManager.equipmentManager.ShowWeaponsByNumber(currentWeaponNumber);
    //}
    private void SaveData()
    {
        SaveState();
        PlayerPrefs.SetFloat($"DungeonUpgradeItem{index}generalIncreaseValue", generalIncreaseValue);
        PlayerPrefs.SetFloat($"DungeonUpgradeItem{index}price", price);
        PlayerPrefs.SetFloat($"DungeonUpgradeItem{index}increaseValue", increaseValue);
        PlayerPrefs.SetFloat($"DungeonUpgradeItem{index}buysCount", buysCount);
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
        MoneyHandler.Instance.IncreaseMoneyPerSecondValue(increaseValue);
        UpdateItemValues();
        UpdateView();
        PlayJumpAnimation();
        CheckOnCollapse();
    }


    private void CheckOnCollapse()
    {
        if (buysCount >= buysEdgeCount)
        {
            ChangeState(PanelItemState.Collapsed);

            if (connectPanel.currentState == PanelItemState.Collapsed)
            {
                if(nextUpgradeItem != null)
                    nextUpgradeItem.ChangeState(PanelItemState.Available);
                else
                    Debug.Log("Next upgrade is null");
                return;
            }
        }
    }

    public void ChangeState(PanelItemState newState)
    {
        currentState = newState;

        if (currentState == PanelItemState.Collapsed)
        {
            CollapseItemView();
            PlayerPrefs.SetString("DungeonUpgradeItem" + index, "collapsed");
        }

        if (currentState == PanelItemState.Unknown)
        {
            SetUnknownItemView();
            PlayerPrefs.SetString("DungeonUpgradeItem" + index, "unknown");
        }

        if (currentState == PanelItemState.Available)
        {
            SetAvailableItemView();
            PlayerPrefs.SetString("DungeonUpgradeItem" + index, "available");
        }
    }

    //  unkown
    [ContextMenu("UnkownPanel")]
    public void UnkownPanel()
    {
        SetUnknownItemView();
        PlayerPrefs.SetString("DungeonUpgradeItem" + index, "unknown");
    }

    private void SaveState()
    {
        if (currentState == PanelItemState.Collapsed)
        {
            PlayerPrefs.SetString("DungeonUpgradeItem" + index, "collapsed");
        }

        if (currentState == PanelItemState.Unknown)
        {
            PlayerPrefs.SetString("DungeonUpgradeItem" + index, "unknown");
        }

        if (currentState == PanelItemState.Available)
        {
            PlayerPrefs.SetString("DungeonUpgradeItem" + index, "available");
        }
    }

    public void ChangeStateViaLoader(PanelItemState newState)
    {
        Initialize();
        generalIncreaseValue = (int)PlayerPrefs.GetFloat($"DungeonUpgradeItem{index}generalIncreaseValue");
        price = (int)PlayerPrefs.GetFloat($"DungeonUpgradeItem{index}price", (int)costCurve.Evaluate(buysCount));
        increaseValue = (int)PlayerPrefs.GetFloat($"DungeonUpgradeItem{index}increaseValue", increaseValue);
        buysCount = (int)PlayerPrefs.GetFloat($"DungeonUpgradeItem{index}buysCount");
        currentState = newState;

        if (currentState == PanelItemState.Collapsed)
        {
            CollapseItemView();
            PlayerPrefs.SetString("DungeonUpgradeItem" + index, "collapsed");
        }

        if (currentState == PanelItemState.Unknown)
        {
            SetUnknownItemView();
            PlayerPrefs.SetString("DungeonUpgradeItem" + index, "unknown");
        }

        if (currentState == PanelItemState.Available)
        {
            SetAvailableItemView();
            PlayerPrefs.SetString("DungeonUpgradeItem" + index, "available");
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

        if (currentPanelState == CurrentPanel.parent)
        {
            if (connectPanel != null)
                connectPanel.ChangeStateViaLoader(PanelItemState.Available);
            else
                Debug.Log("Child panel = null " + connectPanel);
        }

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

        if (currentPanelState == CurrentPanel.parent)
        {
            if (currentObject == ArmorOrEnemy.enemy)
            {
                SkinsManager.Instance.skinCount += 1;
                Debug.Log("Enemy");
            }

            if(currentObject == ArmorOrEnemy.armor)
            {
                SkinsManager.Instance.currentSkinIndex +=1;
                SkinsManager.Instance.ChangeSkin();
                Debug.Log("Armor");
            }
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
