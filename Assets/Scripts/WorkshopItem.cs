using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopItem : MonoBehaviour, IBuyableItem
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject buyButton;
    private Button buyButtonComponent;
    private Image buyButtonImage;
    public Color buttonDefaultColor;
    [SerializeField] private int index;
    [SerializeField] private int price;
    [SerializeField] private GameObject itemIcon;
    [SerializeField] private Animator iconAnimator;

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

    private void Awake()
    {
        buyButtonComponent = buyButton.GetComponent<Button>();
        buyButtonImage = buyButton.GetComponent<Image>();
        iconAnimator = itemIcon.GetComponent<Animator>();
        priceText.text = "$" + FormatNumsHelper.FormatNum((float)price);
    }

    public void BuyItem()
    {
        RoomObjectsHandler.Instance.UnlockObject(index);
        CollapseItemView();
        iconAnimator.Play("Jump", 0, 0);
    }

    public void CollapseItemView()
    {
        buyButton.SetActive(false);
        GetComponent<RectTransform>().sizeDelta = new Vector2(680, 76);
    }
}
