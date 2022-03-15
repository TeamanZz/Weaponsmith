using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopItem : MonoBehaviour, IBuyableItem
{
    [SerializeField] private TextMeshProUGUI priceText;
    public GameObject buyButton;
    private Button buyButtonComponent;
    private Image buyButtonImage;
    public Color buttonDefaultColor;
    [SerializeField] private int index;
    public long price;
    [SerializeField] private GameObject itemIcon;
    [SerializeField] private Animator iconAnimator;

    public List<GameObject> objectsToReplace = new List<GameObject>();

    [HideInInspector] public bool wasBoughted;

    [Header("Dungeon settings")]
    public static int dungeoonIsOpen = 0;
    public static int enchantmentIsOpen = 0;
    public PanelType currentType;
    public enum PanelType
    {
        anOrdinaryItem,
        anDungeonItem,
        anEnchantmentTableItem
    }

    private void Awake()
    {
        buyButtonComponent = buyButton.GetComponent<Button>();
        buyButtonImage = buyButton.GetComponent<Image>();
        iconAnimator = itemIcon.GetComponent<Animator>();
        priceText.text = "$" + FormatNumsHelper.FormatNum((double)price);
        dungeoonIsOpen = PlayerPrefs.GetInt("dungeoonIsOpen");
        enchantmentIsOpen = PlayerPrefs.GetInt("enchantmentIsOpen");
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

    public void BuyItem()
    {
        MoneyHandler.Instance.moneyCount -= price;
        WorkshopPanelItemsManager.Instance.UnlockObject(index);
        if (currentType == PanelType.anDungeonItem)
        {
            PlayerPrefs.SetInt("dungeoonIsOpen", 1);
            dungeoonIsOpen = 1;
            PanelsHandler.Instance.EnableDungeonButton();
        }

        if (currentType == PanelType.anEnchantmentTableItem)
        {
            PlayerPrefs.SetInt("enchantmentIsOpen", 1);
            enchantmentIsOpen = 1;
            PanelsHandler.Instance.EnableBoostersButton();
        }

        CollapseItemView();
        iconAnimator.Play("Jump", 0, 0);
        PlayerPrefs.SetString("WorkshopGameobject" + index, "unlocked");
    }

    public void ReplaceOldObjects()
    {
        for (int i = 0; i < objectsToReplace.Count; i++)
        {
            objectsToReplace[i].SetActive(false);
        }
    }

    public void CollapseItemView()
    {
        buyButton.SetActive(false);
        GetComponent<RectTransform>().sizeDelta = new Vector2(680, 76);
        wasBoughted = true;
        WorkshopPanelItemsManager.Instance.CheckBeforeTransition();
    }
}