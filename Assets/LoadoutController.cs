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
    public List<Image> fill = new List<Image>();

    public float fillingTime = 0.5f;

    public int[] improvementScoreCounter = { 0, 0, 0 };
    public int pointValue = 5;
    public int maxCount = 8;
    public GameObject[] improvementButton;
    
    public void Awake()
    {
        loadoutController = this;
        Initialization();
    }

    [ContextMenu("Initialization")]
    public void Initialization()
    {
        Debug.Log("Initialization");
        FillTheBar(fill[0], 50);
        FillTheBar(fill[1], 0);
        FillTheBar(fill[2], 0);

        foreach (var button in improvementButton)
            button.SetActive(false);

        UpdateImprovementPoints();
    }

    public void UpdateImprovementPoints()
    {
        if (MoneyHandler.Instance == null)
            return;

        if (MoneyHandler.Instance.currentImprovementPoints <= 0)
            return;

        for (int i = 0; i < improvementScoreCounter.Length; i++)
        {
            if (improvementScoreCounter[i] < maxCount)
                improvementButton[i].SetActive(true);
        }
    }

    public void UseExperiencePoints(int number)
    {
        improvementScoreCounter[number] += 1;
        MoneyHandler.Instance.currentImprovementPoints -= 1;

        switch (number)
        {
            case 0:
                FillTheBar(fill[number], health);
                break;

            case 1:
                FillTheBar(fill[number], protection);
                break;

            case 2:
                FillTheBar(fill[number], damage);
                break;
        }

        if (improvementScoreCounter[number] >= 8)
            improvementButton[number].SetActive(false);

        if (MoneyHandler.Instance.currentImprovementPoints <= 0)
            foreach (var button in improvementButton)
                button.SetActive(false);

    }

    public void FillTheBar(Image fillingImage, float fillingLevel)
    {
        int number = -1;
        for (int i = 0; i < fill.Count; i++)
        {
            if (fill[i] == fillingImage)
            {
                number = i;
                break;
            }
        }

        Debug.Log("" + number);
        if (number >= 0)
            fillingLevel = fillingLevel + (improvementScoreCounter[number] * pointValue);
        Debug.Log(fillingLevel);
        fillingLevel /= 100;
        DOTween.To(() => fillingImage.fillAmount, x => fillingImage.fillAmount = x, fillingLevel, fillingTime);
    }
}