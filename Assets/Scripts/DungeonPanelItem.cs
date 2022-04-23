using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
public class DungeonPanelItem : MonoBehaviour
{
    [Header("Initialize parameters")]
    public PanelItemState currentState;
    public DungeonUpgradeType upgradeType;
    public CurrentPanel currentPanelState;
    public int index;
    public string itemName;
    [SerializeField] private int increaseValue;
    public DungeonPanelItem connectPanel;
    public DungeonPanelItem nextUpgradeItem;
    public AnimationCurve costCurve;

    [FoldoutGroup("Current Runtime Values")] public int buysCount;
    [FoldoutGroup("Current Runtime Values")][SerializeField] private int generalIncreaseValue;

    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI itemNameText;
    // [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI generalIncreaseValueText;
    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI priceText;
    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI buysCountText;
    [FoldoutGroup("View Components")][SerializeField] private GameObject buyButton;

    [FoldoutGroup("View Components")][SerializeField] private GameObject progressBar;
    [FoldoutGroup("View Components")][SerializeField] private GameObject completedSign;
    [FoldoutGroup("View Components")][SerializeField] private GameObject itemIcon;
    [FoldoutGroup("View Components")][SerializeField] private GameObject unknownSign;
    [FoldoutGroup("View Components")][SerializeField] private GameObject blurPanel;
    [FoldoutGroup("View Components")][SerializeField] private GameObject itemConditionsGO;
    [FoldoutGroup("View Components")][SerializeField] private Image progressBarFilled;

    private Animator buyButtonAnimator;
    private Animator iconAnimator;
    private Animator generalIncreaseValueTextAnimator;
    private Animator buysCountTextAnimator;
    private Button buyButtonComponent;
    private Image buyButtonImage;
    private int price;

    [SerializeField] private Color buttonDefaultColor;

    private void Awake()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        HandleBuyButtonAvailability();
    }

    private void Start()
    {
        InvokeRepeating("SaveData", 3, 3);

        if (buysCount >= costCurve.keys[costCurve.length - 1].time)
            ChangeState(PanelItemState.Collapsed);
    }

    private void Initialize()
    {
        buyButtonComponent = buyButton.GetComponent<Button>();
        buyButtonImage = buyButton.GetComponent<Image>();
        buyButtonAnimator = buyButton.GetComponent<Animator>();
        iconAnimator = itemIcon.GetComponent<Animator>();
        // generalIncreaseValueTextAnimator = generalIncreaseValueText.GetComponent<Animator>();
        buysCountTextAnimator = buysCountText.GetComponent<Animator>();

        if (currentState != PanelItemState.Unknown)
            SetItemName();
        price = (int)costCurve.Evaluate(buysCount);
        UpdateView();
    }

    private void HandleBuyButtonAvailability()
    {
        if (MoneyHandler.Instance == null)
            return;

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

    public void HandleUpgradeOnBuy()
    {
        if (upgradeType == DungeonUpgradeType.Witch)
        {
            if (buysCount == 1)
            {
                SkinsManager.Instance.dungeonEnemySkinCount++;
                // PlayerPrefs.SetInt("EnemySkinCount", SkinsManager.Instance.dungeonEnemySkinCount);
            }
        }

        if (upgradeType == DungeonUpgradeType.Golems)
        {
            if (buysCount == 1)
            {
                SkinsManager.Instance.dungeonEnemySkinCount++;
                // PlayerPrefs.SetInt("EnemySkinCount", SkinsManager.Instance.dungeonEnemySkinCount);
            }
        }

        if (upgradeType == DungeonUpgradeType.WeaponSkills)
        {
            if (buysCount == costCurve.keys[costCurve.length - 1].time || buysCount == costCurve.keys[costCurve.length - 1].time / 2)
            {
                DungeonCharacter.Instance.IncreaseAllowedAttackAnimationsCount();
            }
        }

        // if (upgradeType == DungeonUpgradeType.Agility)
        // {
        //     if (buysCount == 1)
        //     {
        //         DungeonCharacter.Instance.EnableWeaponTrail();
        //     }
        // }

        if (upgradeType == DungeonUpgradeType.CriticalDamage)
        {
            if (buysCount == 1)
            {
                DungeonCharacter.Instance.EnableCriticalHit();
            }
        }

        if (upgradeType == DungeonUpgradeType.Armor)
        {
            if (buysCount == 1)
            {
                SkinsManager.Instance.currentSkinIndex += 1;
                // PlayerPrefs.SetInt("skinIndex", SkinsManager.Instance.currentSkinIndex);
                SkinsManager.Instance.ChangeSkin();
            }
        }
    }

    [Button("Buy Upgrade")]
    public void BuyItem()
    {
        MoneyHandler.Instance.moneyCount -= price;
        MoneyHandler.Instance.IncreaseMoneyPerSecondValue(increaseValue);
        UpdateItemValues();
        UpdateView();
        PlayJumpAnimation();
        CheckOnCollapse();
        HandleUpgradeOnBuy();
    }

    private void CheckOnCollapse()
    {
        if (buysCount >= costCurve.keys[costCurve.length - 1].time)
        {
            SFX.Instance.PlayQuestComplete();
            ChangeState(PanelItemState.Collapsed);

            if (connectPanel.currentState == PanelItemState.Collapsed)
            {
                if (nextUpgradeItem != null)
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

            // PlayerPrefs.SetString("DungeonUpgradeItem" + index, "collapsed");
        }

        if (currentState == PanelItemState.Unknown)
        {
            SetUnknownItemView();
            // PlayerPrefs.SetString("DungeonUpgradeItem" + index, "unknown");
        }

        if (currentState == PanelItemState.Available)
        {
            SetAvailableItemView();
            // PlayerPrefs.SetString("DungeonUpgradeItem" + index, "available");
        }
    }

    private void SaveState()
    {
        if (currentState == PanelItemState.Collapsed)
        {
            // PlayerPrefs.SetString("DungeonUpgradeItem" + index, "collapsed");
        }

        if (currentState == PanelItemState.Unknown)
        {
            // PlayerPrefs.SetString("DungeonUpgradeItem" + index, "unknown");
        }

        if (currentState == PanelItemState.Available)
        {
            // PlayerPrefs.SetString("DungeonUpgradeItem" + index, "available");
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

        HandleStateOnPrefsLoad();
        UpdateView();
    }

    private void HandleStateOnPrefsLoad()
    {
        if (currentState == PanelItemState.Collapsed)
        {
            CollapseItemView();
            // PlayerPrefs.SetString("DungeonUpgradeItem" + index, "collapsed");
        }

        if (currentState == PanelItemState.Unknown)
        {
            SetUnknownItemView();
            // PlayerPrefs.SetString("DungeonUpgradeItem" + index, "unknown");
        }

        if (currentState == PanelItemState.Available)
        {
            SetAvailableItemView();
            // PlayerPrefs.SetString("DungeonUpgradeItem" + index, "available");
        }
    }

    private void SaveData()
    {
        // SaveState();
        // PlayerPrefs.SetFloat($"DungeonUpgradeItem{index}generalIncreaseValue", generalIncreaseValue);
        // PlayerPrefs.SetFloat($"DungeonUpgradeItem{index}price", price);
        // PlayerPrefs.SetFloat($"DungeonUpgradeItem{index}increaseValue", increaseValue);
        // PlayerPrefs.SetFloat($"DungeonUpgradeItem{index}buysCount", buysCount);
    }

    //Unknown
    public void SetUnknownItemView()
    {
        itemIcon.SetActive(false);
        unknownSign.SetActive(true);
        itemNameText.text = "???";
        progressBar.SetActive(false);
        // generalIncreaseValueText.gameObject.SetActive(false);
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
        // generalIncreaseValueText.gameObject.SetActive(false);
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
            increaseValue = (int)(increaseValue * 1.1f);

        buysCount++;
        price = (int)costCurve.Evaluate(buysCount);
    }

    private void UpdateView()
    {
        // generalIncreaseValueText.text = "+$" + FormatNumsHelper.FormatNum((double)generalIncreaseValue) + "/s";
        priceText.text = "$" + FormatNumsHelper.FormatNum((double)price);

        buysCountText.text = buysCount.ToString() + "/" + costCurve.keys[costCurve.length - 1].time.ToString();
        progressBarFilled.fillAmount = ((float)buysCount / (float)costCurve.keys[costCurve.length - 1].time);
    }

    [ContextMenu("Available state")]
    public void SetAvailableItemView()
    {
        itemConditionsGO.SetActive(false);
        blurPanel.SetActive(false);
        // generalIncreaseValueText.gameObject.SetActive(true);
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

    //  unkown
    [ContextMenu("UnkownPanel")]
    public void UnkownPanel()
    {
        SetUnknownItemView();
        PlayerPrefs.SetString("DungeonUpgradeItem" + index, "unknown");
    }

    public enum CurrentPanel
    {
        parent,
        childObject
    }

    public enum DungeonUpgradeType
    {
        None,
        Goblins,
        Armor,
        Agility,
        Witch,
        WeaponSkills,
        CriticalDamage,
        DungeonDepth,
        Golems
    }
}
