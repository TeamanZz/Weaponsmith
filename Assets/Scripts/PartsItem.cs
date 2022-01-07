using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartsItem : MonoBehaviour
{
    [SerializeField] private int index;
    public int price;
    public Color buttonDefaultColor;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject buyButton;
    private Button buyButtonComponent;
    private Image buyButtonImage;

    private Animator iconAnimator;
    [SerializeField] private GameObject itemIcon;
    [SerializeField] private GameObject itemIconBlack;
    public List<GameObject> oldWeaponVersions = new List<GameObject>();
    [HideInInspector] public bool wasBoughted;

    private void Awake()
    {
        Initialize();
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

    private void Initialize()
    {
        buyButtonComponent = buyButton.GetComponent<Button>();
        buyButtonImage = buyButton.GetComponent<Image>();
        priceText.text = "$" + FormatNumsHelper.FormatNum((float)price);
        iconAnimator = itemIcon.GetComponent<Animator>();
    }

    public void UnlockItem()
    {
        Initialize();
        iconAnimator.Play("Jump", 0, 0);
        CraftManager.Instance.UnlockCraftWeapon(index);
        CollapseItemView();
        HideOldWeaponVersions();
        itemIconBlack.SetActive(false);
        wasBoughted = true;
    }

    public void BuyItem()
    {
        UnlockItem();
        PlayerPrefs.SetString("BlueprintPanelItem" + (index), "unlocked");
    }

    public void HideOldWeaponVersions()
    {
        for (int i = 0; i < oldWeaponVersions.Count; i++)
        {
            oldWeaponVersions[i].SetActive(false);
        }
    }

    public void CollapseItemView()
    {
        buyButton.SetActive(false);
        GetComponent<RectTransform>().sizeDelta = new Vector2(680, 76);
    }

    private void SpawnParticles()
    {
        Instantiate(particles, buyButton.transform.position, Quaternion.identity);
    }
}
