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
    public AnimationCurve costCurve;

    [FoldoutGroup("Current Runtime Values")] public int buysCount;
    [FoldoutGroup("Current Runtime Values")][SerializeField] private int generalIncreaseValue;
    [FoldoutGroup("View Components")][SerializeField] private Button buyButtonComponent;
    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI itemNameText;
    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI priceText;
    [FoldoutGroup("View Components")][SerializeField] private GameObject buyButton;
    [FoldoutGroup("View Components")][SerializeField] private Image buyButtonImage;
    [FoldoutGroup("View Components")][SerializeField] private GameObject activateButton;
    [FoldoutGroup("View Components")][SerializeField] private GameObject itemIcon;
    [FoldoutGroup("View Components")][SerializeField] private GameObject unknownSign;
    [FoldoutGroup("View Components")][SerializeField] private GameObject blurPanel;
    [FoldoutGroup("View Components")][SerializeField] private GameObject itemConditionsGO;
    [FoldoutGroup("View Components")][SerializeField] private Animator buyButtonAnimator;
    [FoldoutGroup("View Components")][SerializeField] private Animator activateButtonAnimator;
    [FoldoutGroup("View Components")][SerializeField] private Animator iconAnimator;
    [FoldoutGroup("View Components")][SerializeField] private Animator generalIncreaseValueTextAnimator;
    [FoldoutGroup("View Components")][SerializeField] private Animator buysCountTextAnimator;
    [FoldoutGroup("View Components")] public GameObject hoverImageGameObject;
    [FoldoutGroup("View Components")] public Color disabledColor;
    [FoldoutGroup("View Components")] public Color enabledColor;
    [FoldoutGroup("View Components")] public Button activateButtonComponent;
    [FoldoutGroup("View Components")][SerializeField] private Color buttonDefaultColor;

    private Image hoverImage;
    private int price;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        hoverImage = hoverImageGameObject.GetComponent<Image>();
        InvokeRepeating("SaveData", 3, 3);

        if (buysCount >= costCurve.keys[costCurve.length - 1].time)
            ChangeState(PanelItemState.Collapsed);
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

    private void Update()
    {
        if (hoverImageGameObject.activeSelf == true)
        {
            if (index == 0)
                hoverImage.fillAmount = DungeonBoostersManager.Instance.goldBoosterRemainingTime;
            if (index == 1)
                hoverImage.fillAmount = DungeonBoostersManager.Instance.speedBoosterRemainingTime;
            if (index == 2)
                hoverImage.fillAmount = DungeonBoostersManager.Instance.strengthBoosterRemainingTime;
        }
    }

    public void StartCooldown()
    {
        hoverImage.fillAmount = 1;
        hoverImageGameObject.SetActive(true);
        activateButtonComponent.enabled = false;
        activateButtonComponent.image.color = disabledColor;
    }

    public void HandleUIOnTimerDisable()
    {
        hoverImageGameObject.SetActive(false);
        activateButtonComponent.enabled = true;
        activateButtonComponent.image.color = enabledColor;
        hoverImage.fillAmount = 1;
    }

    private void SaveData()
    {
        // SaveState();
        // PlayerPrefs.SetFloat($"BoostersUpgradeItem{index}price", price);
        // PlayerPrefs.SetFloat($"BoostersUpgradeItem{index}buysCount", buysCount);
    }

    private void Initialize()
    {
        buyButtonComponent = buyButton.GetComponent<Button>();
        buyButtonImage = buyButton.GetComponent<Image>();
        buyButtonAnimator = buyButton.GetComponent<Animator>();
        iconAnimator = itemIcon.GetComponent<Animator>();
        // hoverIconAnimator = hoverImageGameObject.GetComponent<Animator>();
        activateButtonAnimator = activateButton.GetComponent<Animator>();
        activateButtonComponent = activateButton.GetComponent<Button>();

        if (currentState != PanelItemState.Unknown)
            SetItemName();
        price = (int)costCurve.Evaluate(buysCount);
        UpdateView();
    }

    private void PlayJumpAnimation()
    {
        buyButtonAnimator.Play("Jump", 0, 0);
        iconAnimator.Play("Jump", 0, 0);
    }

    public void PlayJumpAnimationOnActivate()
    {
        iconAnimator.Play("Jump", 0, 0);
        // hoverIconAnimator.Play("Jump", 0, 0);
        activateButtonAnimator.Play("Jump", 0, 0);
    }

    private void SetItemName()
    {
        itemNameText.text = itemName;
    }

    public void BuyItem()
    {
        MoneyHandler.Instance.moneyCount -= price;
        UpdateItemValues();
        UpdateView();
        PlayJumpAnimation();
        CheckOnCollapse();

        SaveData();
    }


    private void CheckOnCollapse()
    {
        if (buysCount >= costCurve.keys[costCurve.length - 1].time)
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
            // PlayerPrefs.SetString("BoostersUpgradeItem" + index, "collapsed");
        }

        if (currentState == PanelItemState.Unknown)
        {
            SetUnknownItemView();
            // PlayerPrefs.SetString("BoostersUpgradeItem" + index, "unknown");
        }

        if (currentState == PanelItemState.Available)
        {
            SetAvailableItemView();
            // PlayerPrefs.SetString("BoostersUpgradeItem" + index, "available");
        }
    }

    //  unkown
    [ContextMenu("UnkownPanel")]
    public void UnkownPanel()
    {
        SetUnknownItemView();
        // PlayerPrefs.SetString("BoostersUpgradeItem" + index, "unknown");
    }

    private void SaveState()
    {
        if (currentState == PanelItemState.Collapsed)
        {
            // PlayerPrefs.SetString("BoostersUpgradeItem" + index, "collapsed");
        }

        if (currentState == PanelItemState.Unknown)
        {
            // PlayerPrefs.SetString("BoostersUpgradeItem" + index, "unknown");
        }

        if (currentState == PanelItemState.Available)
        {
            // PlayerPrefs.SetString("BoostersUpgradeItem" + index, "available");
        }
    }

    public void ChangeStateViaLoader(PanelItemState newState)
    {
        Initialize();
        generalIncreaseValue = (int)PlayerPrefs.GetFloat($"BoostersUpgradeItem{index}generalIncreaseValue");
        price = (int)PlayerPrefs.GetFloat($"BoostersUpgradeItem{index}price", (int)costCurve.Evaluate(buysCount));
        buysCount = (int)PlayerPrefs.GetFloat($"BoostersUpgradeItem{index}buysCount");
        currentState = newState;
        if (currentState == PanelItemState.Collapsed)
        {
            CollapseItemView();
            // PlayerPrefs.SetString("BoostersUpgradeItem" + index, "collapsed");
        }

        if (currentState == PanelItemState.Unknown)
        {
            SetUnknownItemView();
            // PlayerPrefs.SetString("BoostersUpgradeItem" + index, "unknown");
        }

        if (currentState == PanelItemState.Available)
        {
            SetAvailableItemView();
            // PlayerPrefs.SetString("BoostersUpgradeItem" + index, "available");
        }

        UpdateView();
    }

    //open panel
    [ContextMenu("Available state")]
    public void SetAvailableItemView()
    {
        itemConditionsGO.SetActive(false);
        blurPanel.SetActive(false);
        unknownSign.SetActive(false);
        buyButton.SetActive(true);
        activateButton.SetActive(true);
        itemIcon.SetActive(true);
        itemNameText.text = itemName;
    }

    //Unknown
    public void SetUnknownItemView()
    {
        itemIcon.SetActive(false);
        unknownSign.SetActive(true);
        itemNameText.text = "???";
        buyButton.SetActive(false);
        activateButton.SetActive(false);
        blurPanel.SetActive(true);
    }

    //collapse
    public void CollapseItemView()
    {
        unknownSign.SetActive(false);
        blurPanel.SetActive(false);
        itemNameText.text = itemName;
        buyButton.SetActive(false);
        activateButton.SetActive(true);
        itemIcon.SetActive(true);
    }

    private void UpdateItemValues()
    {
        buysCount++;
    }

    private void UpdateView()
    {
        priceText.text = "$" + FormatNumsHelper.FormatNum((double)price);
    }
}
