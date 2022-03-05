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
    [SerializeField] private int buysEdgeCount;
    public AnimationCurve costCurve;

    private int price;
    [SerializeField] private Color buttonDefaultColor;

    [FoldoutGroup("Current Runtime Values")] public int buysCount;
    [FoldoutGroup("Current Runtime Values")][SerializeField] private int generalIncreaseValue;
    [FoldoutGroup("View Components")][SerializeField] private Button buyButtonComponent;
    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI itemNameText;
    // [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI generalIncreaseValueText;
    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI priceText;
    // [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI buysCountText;
    [FoldoutGroup("View Components")][SerializeField] private GameObject buyButton;
    [FoldoutGroup("View Components")][SerializeField] private Image buyButtonImage;
    // [FoldoutGroup("View Components")][SerializeField] private GameObject progressBar;
    [FoldoutGroup("View Components")][SerializeField] private GameObject activateButton;
    [FoldoutGroup("View Components")][SerializeField] private GameObject itemIcon;
    [FoldoutGroup("View Components")][SerializeField] private GameObject unknownSign;
    [FoldoutGroup("View Components")][SerializeField] private GameObject blurPanel;
    [FoldoutGroup("View Components")][SerializeField] private GameObject itemConditionsGO;
    // [FoldoutGroup("View Components")][SerializeField] private Image progressBarFilled;
    [FoldoutGroup("View Components")][SerializeField] private Animator buyButtonAnimator;
    [FoldoutGroup("View Components")][SerializeField] private Animator activateButtonAnimator;
    [FoldoutGroup("View Components")][SerializeField] private Animator iconAnimator;
    [FoldoutGroup("View Components")][SerializeField] private Animator hoverIconAnimator;
    [FoldoutGroup("View Components")][SerializeField] private Animator generalIncreaseValueTextAnimator;
    [FoldoutGroup("View Components")][SerializeField] private Animator buysCountTextAnimator;

    public GameObject hoverImageGameObject;
    public Color disabledColor;
    public Color enabledColor;
    public Button activateButtonComponent;
    private Image hoverImage;

    private void Update()
    {
        if (hoverImageGameObject.activeSelf == true)
        {
            if (index == 0)
                hoverImage.fillAmount = DungeonBoostersManager.Instance.goldBoosterRemainingTime;
            // if (index == 1)
            //     hoverImage.fillAmount = DungeonBoostersManager.Instance.speedBoosterRemainingTime;
            // if (index == 2)
            //     hoverImage.fillAmount = DungeonBoostersManager.Instance.strengthBoosterRemainingTime;
        }
    }

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
        hoverImage = hoverImageGameObject.GetComponent<Image>();
        InvokeRepeating("SaveData", 3, 3);

        if (buysCount >= buysEdgeCount)
            ChangeState(PanelItemState.Collapsed);
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
        SaveState();
        PlayerPrefs.SetFloat($"BoostersUpgradeItem{index}generalIncreaseValue", generalIncreaseValue);
        PlayerPrefs.SetFloat($"BoostersUpgradeItem{index}price", price);
        PlayerPrefs.SetFloat($"BoostersUpgradeItem{index}buysCount", buysCount);
    }

    private void Initialize()
    {
        buyButtonComponent = buyButton.GetComponent<Button>();
        buyButtonImage = buyButton.GetComponent<Image>();
        buyButtonAnimator = buyButton.GetComponent<Animator>();
        iconAnimator = itemIcon.GetComponent<Animator>();
        hoverIconAnimator = hoverImageGameObject.GetComponent<Animator>();
        activateButtonAnimator = activateButton.GetComponent<Animator>();
        activateButtonComponent = activateButton.GetComponent<Button>();
        // generalIncreaseValueTextAnimator = generalIncreaseValueText.GetComponent<Animator>();
        // buysCountTextAnimator = buysCountText.GetComponent<Animator>();

        if (currentState != PanelItemState.Unknown)
            SetItemName();
        price = (int)costCurve.Evaluate(buysCount);
        UpdateView();
    }

    private void PlayJumpAnimation()
    {
        buyButtonAnimator.Play("Jump", 0, 0);
        iconAnimator.Play("Jump", 0, 0);
        // generalIncreaseValueTextAnimator.Play("Jump", 0, 0);
        // buysCountTextAnimator.Play("Jump", 0, 0);
    }

    public void PlayJumpAnimationOnActivate()
    {
        iconAnimator.Play("Jump", 0, 0);
        hoverIconAnimator.Play("Jump", 0, 0);
        activateButtonAnimator.Play("Jump", 0, 0);
    }

    private void SetItemName()
    {
        itemNameText.text = itemName;
    }

    //  ���
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
        // generalIncreaseValueText.gameObject.SetActive(true);
        unknownSign.SetActive(false);
        // progressBar.SetActive(true);
        buyButton.SetActive(true);
        activateButton.SetActive(false);
        itemIcon.SetActive(true);
        itemNameText.text = itemName;
    }

    //Unknown
    public void SetUnknownItemView()
    {
        itemIcon.SetActive(false);
        unknownSign.SetActive(true);
        itemNameText.text = "???";
        // progressBar.SetActive(false);
        // generalIncreaseValueText.gameObject.SetActive(false);
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
        // progressBar.SetActive(false);
        // generalIncreaseValueText.gameObject.SetActive(false);
        buyButton.SetActive(false);
        activateButton.SetActive(true);
        itemIcon.SetActive(true);
        // GetComponent<RectTransform>().sizeDelta = new Vector2(680, 76);
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
