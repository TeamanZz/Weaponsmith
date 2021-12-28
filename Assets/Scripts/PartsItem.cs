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
    private void Awake()
    {
        buyButtonComponent = buyButton.GetComponent<Button>();
        buyButtonImage = buyButton.GetComponent<Image>();
        priceText.text = "$" + FormatNumsHelper.FormatNum((float)price);

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
