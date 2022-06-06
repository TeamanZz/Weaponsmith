using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class WorkshopItem : MonoBehaviour, IBuyableItem
{
    [Header("Save Settings")]
    public int panelID;
    public bool panelIsOpen = false;

    public WorkshopPanelItemsManager workshopManager;

    [SerializeField] private int index;
    public long price;
    public int generalIncreaseValue;
    public PanelType currentType;
    public List<GameObject> objectsToReplace = new List<GameObject>();
    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI priceText;
    [FoldoutGroup("View Components")][SerializeField] private TextMeshProUGUI generalIncreaseValueText;
    [FoldoutGroup("View Components")][SerializeField] private Animator generalIncreaseValueTextAnimator;
    [FoldoutGroup("View Components")] public GameObject buyButton;
    [FoldoutGroup("View Components")][SerializeField] private GameObject itemIcon;
    [FoldoutGroup("View Components")][SerializeField] private Animator iconAnimator;
    [FoldoutGroup("View Components")] public Color buttonDefaultColor;
    [HideInInspector] public bool wasBoughted;

    private Button buyButtonComponent;
    private Image buyButtonImage;

    private void Awake()
    {
        buyButtonComponent = buyButton.GetComponent<Button>();
        buyButtonImage = buyButton.GetComponent<Image>();
        iconAnimator = itemIcon.GetComponent<Animator>();
        generalIncreaseValueTextAnimator = generalIncreaseValueText.GetComponent<Animator>();

        priceText.text = "$" + FormatNumsHelper.FormatNum((double)price);

        generalIncreaseValueText.text = "+$" + FormatNumsHelper.FormatNum((double)generalIncreaseValue) + "/s";
        LoadData();
    }

    private void Update()
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

    bool IntToBool(int n)
    => n == 1;
    int BoolToInt(bool b)
        => (b ? 1 : 0);

    [ContextMenu("Save Data")]
    private void SaveData()
    {
        PlayerPrefs.SetInt($"WorkshopItem{panelID}State", BoolToInt(panelIsOpen));
        //Debug.Log("Save Data = " + panelID + " State =" + panelIsOpen);
    }

    public void LoadData()
    {
        panelIsOpen = IntToBool(PlayerPrefs.GetInt($"WorkshopItem{panelID}State"));

        if (panelIsOpen == false)
            CollapseItemView();
        else
            SetAvailableItemView();

        //Debug.Log("Load Data = " + panelID + " State =" + panelIsOpen);
    }

    [ContextMenu("Remove Data")]
    public void RemoveData()
    {
        PlayerPrefs.SetInt($"WorkshopItem{panelID}State", BoolToInt(true));

        //Debug.Log("Remove Data = " + panelID + " State =" + price);

        SetAvailableItemView();
       // Awake();
    }


    [Header("Focus")]
    public float focusField = 30f;
    public void BuyItem()
    {
        MoneyHandler.Instance.moneyCount -= price;
        WorkshopPanelItemsManager.Instance.PreUnlock(index, focusField);

        MoneyHandler.Instance.IncreaseMoneyPerSecondValue(generalIncreaseValue);
        generalIncreaseValueTextAnimator.Play("Jump", 0, 0);

        generalIncreaseValueText.text = "";

        CollapseItemView();
        SaveData();
        MoneyHandler.Instance.SaveData();
        iconAnimator.Play("Jump", 0, 0);
    }

    public void ReplaceOldObjects()
    {
        for (int i = 0; i < objectsToReplace.Count; i++)
            objectsToReplace[i].SetActive(false);
    }

    public void SetAvailableItemView()
    {
        panelIsOpen = true;
        buyButton.SetActive(true);
        wasBoughted = false;
        generalIncreaseValueText.text = "+$" + FormatNumsHelper.FormatNum((double)generalIncreaseValue) + "/s";
        GetComponent<RectTransform>().sizeDelta = new Vector2(710, 120);
    }

    public void CollapseItemView()
    {
        panelIsOpen = false;
        buyButton.SetActive(false);
        generalIncreaseValueText.text = "";
        GetComponent<RectTransform>().sizeDelta = new Vector2(680, 76);
        wasBoughted = true;
        //workshopManager.CheckBeforeTransition();
    }

    public enum PanelType
    {
        anOrdinaryItem,
        anDungeonItem,
        anEnchantmentTableItem
    }
}