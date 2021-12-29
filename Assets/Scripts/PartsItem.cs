using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartsItem : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject buyButton;
    private Button buyButtonComponent;
    private Image buyButtonImage;
    public Color buttonDefaultColor;

    [SerializeField] private int price;
    [SerializeField] private GameObject particles;

    [SerializeField] private Animator iconAnimator;
    [SerializeField] private GameObject itemIcon;

    private void Awake()
    {
        buyButtonComponent = buyButton.GetComponent<Button>();
        buyButtonImage = buyButton.GetComponent<Image>();
        priceText.text = "$" + FormatNumsHelper.FormatNum((float)price);
        iconAnimator = itemIcon.GetComponent<Animator>();

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
        iconAnimator.Play("Weapon Jump", 0, 0);
        // SpawnParticles();
        CraftManager.Instance.UnlockCraftWeapon(index);
        CollapseItemView();
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
