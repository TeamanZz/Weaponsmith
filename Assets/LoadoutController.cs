using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutController : MonoBehaviour
{
    public static LoadoutController loadoutController;
    [Header("Data")]
    public float health = 50f;
    public float protection = 0f;
    public float damage = 0f;

    [Header("UI")]
    public Image healthFill;
    public Image protectionFill;
    public Image damageFill;

    public float fillingTime = 0.2f;
    public void Awake()
    {
        loadoutController = this;
        Initialization();
    }

    public void Initialization()
    {
        FillTheBar(healthFill, health);
        FillTheBar(protectionFill, protection);
        FillTheBar(damageFill, damage);
    }

    public void FillTheBar(Image fillingImage, float fillingLevel)
    {
        fillingLevel /= 100;
        DOTween.To(() => fillingImage.fillAmount, x => fillingImage.fillAmount = x, fillingLevel, fillingTime);
    }
}
