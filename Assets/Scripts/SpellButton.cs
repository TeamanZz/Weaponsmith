using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour
{
    public GameObject hoverImageGameObject;
    public Button activateButton;
    public Color disabledColor;
    public Color enabledColor;

    private Image hoverImage;
    public int index;

    public void StartCooldown()
    {
        hoverImage.fillAmount = 1;
        hoverImageGameObject.SetActive(true);
        activateButton.enabled = false;
        activateButton.image.color = disabledColor;
    }

    private void Start()
    {
        hoverImage = hoverImageGameObject.GetComponent<Image>();
    }

    public void HandleUIOnTimerDisable()
    {
        hoverImageGameObject.SetActive(false);
        activateButton.enabled = true;
        activateButton.image.color = enabledColor;
        hoverImage.fillAmount = 1;
    }

    private void Update()
    {
        if (hoverImageGameObject.activeSelf == true)
        {
            if (index == 0)
                hoverImage.fillAmount = DungeonBoostersManager.Instance.goldBoosterRemainingTime;
            if (index == 1)
                hoverImage.fillAmount = DungeonBoostersManager.Instance.speedBoosterRemainingTime;
            // if (index == 2)
            //     hoverImage.fillAmount = DungeonBoostersManager.Instance.strengthBoosterRemainingTime;
        }
    }
}