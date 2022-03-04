using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour
{
    public GameObject hoverImageGameObject;
    public GameObject sparkle;
    public float cooldownTime;

    private Image hoverImage;

    public void StartCooldown()
    {
        hoverImage.fillAmount = 1;
        hoverImageGameObject.SetActive(true);
    }

    private void Start()
    {
        hoverImage = hoverImageGameObject.GetComponent<Image>();
    }

    private void Update()
    {
        if (hoverImageGameObject.activeSelf == true)
        {
            hoverImage.fillAmount -= Time.deltaTime / cooldownTime;
            if (hoverImage.fillAmount <= 0)
            {
                hoverImageGameObject.SetActive(false);
                sparkle.SetActive(true);
            }
        }
    }

    // public void HandleGoldButton()
    // {

    // }

    // public void HandleSpeedButton()
    // {

    // }

    // public void HandleStrengthButton()
    // {

    // }
}
