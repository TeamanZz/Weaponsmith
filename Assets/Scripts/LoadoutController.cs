using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutController : MonoBehaviour
{
    public static LoadoutController Instance;

    [Header("UI")]
    public List<Image> fill = new List<Image>();

    public float fillingTime = 0.5f;

    public int[] improvementScoreCounter = { 0, 0, 0 };
    public int pointValue = 5;
    public int maxCount = 8;
    public GameObject[] improvementButton;

    public DungeonHubManager hubManager;

    public void Awake()
    {
        Instance = this;
        Initialization();
    }

    public void InitializeCharacterStats()
    {
        DungeonCharacter.Instance.InitializeStats(improvementScoreCounter[0] * pointValue, improvementScoreCounter[1] * pointValue, improvementScoreCounter[2] * pointValue);
    }

    [ContextMenu("Initialization")]
    public void Initialization()
    {
        FillTheBar(fill[0], (improvementScoreCounter[0] * pointValue));

        if (hubManager.armorActivatePanel == null)
            FillTheBar(fill[1], (improvementScoreCounter[1] * pointValue));
        else
            FillTheBar(fill[1], hubManager.armorActivatePanel.value + (improvementScoreCounter[1] * pointValue));

        if (hubManager.weaponActivatePanel == null)
            FillTheBar(fill[2], (improvementScoreCounter[2] * pointValue));
        else
            FillTheBar(fill[2], hubManager.weaponActivatePanel.value + (improvementScoreCounter[2] * pointValue));

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
                FillTheBar(fill[number], (improvementScoreCounter[number] * pointValue));
                break;

            case 1:
                if (hubManager.armorActivatePanel == null)
                    FillTheBar(fill[number], (improvementScoreCounter[number] * pointValue));
                else
                    FillTheBar(fill[number], hubManager.armorActivatePanel.value + (improvementScoreCounter[number] * pointValue));
                break;

            case 2:
                if (hubManager.weaponActivatePanel == null)
                    FillTheBar(fill[number], (improvementScoreCounter[number] * pointValue));
                else
                    FillTheBar(fill[number], hubManager.weaponActivatePanel.value + (improvementScoreCounter[number] * pointValue));
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

        if (number >= 0)
            fillingLevel = fillingLevel + (improvementScoreCounter[number] * pointValue);
        fillingLevel /= 3000;
        DOTween.To(() => fillingImage.fillAmount, x => fillingImage.fillAmount = x, fillingLevel, fillingTime);
    }
}
