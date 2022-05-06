using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadoutController : MonoBehaviour
{
    public static LoadoutController Instance;

    [Header("UI")]
    public List<Image> fill = new List<Image>();
    public List<TextMeshProUGUI> fillValue = new List<TextMeshProUGUI>();

    public float fillingTime = 0.5f;

    public int[] improvementScoreCounter = { 0, 0, 0 };
    public int pointValue = 5;
    public int maxCount = 8;
    public GameObject[] improvementButton;

    public DungeonHubManager hubManager;
    public ParticleSystem selectedParticle;

    public void Awake()
    {
        Instance = this;
        Initialization();

        foreach (var button in improvementButton)
            button.SetActive(false);
    }

    public void InitializeCharacterStats()
    {
        var HPSkill = skillController.skills[0];
        var damageSkill = skillController.skills[1];
        var armorSkill = skillController.skills[2];

        int hpValue;
        int armorValue;
        int damageValue;

        FillTheBar(fill[0], 100);
        hpValue = (int)((improvementScoreCounter[0] * pointValue) + HPSkill.skillValue[HPSkill.skillLvl]);
        fillValue[0].text = hpValue.ToString();

        if (hubManager.armorActivatePanel == null)
        {
            float value = armorSkill.skillValue[armorSkill.skillLvl];
            armorValue = (int)value;
            fillValue[1].text = value.ToString();

            if (value == 0)
                FillTheBar(fill[1], 0);
            else
                FillTheBar(fill[1], 100);
        }
        else
        {
            // FillTheBar(fill[1], hubManager.armorActivatePanel.value + (improvementScoreCounter[1] * pointValue));
            FillTheBar(fill[1], 100); 
            
            float value = armorSkill.skillValue[armorSkill.skillLvl];
            armorValue = (int)((hubManager.armorActivatePanel.value) + value);

            fillValue[1].text = ((hubManager.armorActivatePanel.value) + value).ToString();
        }

        if (hubManager.weaponActivatePanel == null)
        {
            float value = damageSkill.skillValue[damageSkill.skillLvl];
            damageValue = (int)value;
            fillValue[2].text = value.ToString();

            if (value == 0)
                FillTheBar(fill[2], 0);
            else
                FillTheBar(fill[2], 100);
        }
        else
        {
            //FillTheBar(fill[2], hubManager.weaponActivatePanel.value + (improvementScoreCounter[2] * pointValue));
            FillTheBar(fill[2], 100);

            float value = damageSkill.skillValue[damageSkill.skillLvl];
            damageValue = (int)((hubManager.weaponActivatePanel.value) + value);

            fillValue[2].text = ((hubManager.weaponActivatePanel.value) + value).ToString();
        }

        DungeonCharacter.Instance.InitializeStats(hpValue, armorValue, damageValue);
    }

    public SkillController skillController;
    [ContextMenu("Initialization")]
    public void Initialization()
    {
        var HPSkill = skillController.skills[0];
        var damageSkill = skillController.skills[1];
        var armorSkill = skillController.skills[2];

        FillTheBar(fill[0], 100);
        fillValue[0].text = ((improvementScoreCounter[0] * pointValue) + HPSkill.skillValue[HPSkill.skillLvl]).ToString();

        if (hubManager.armorActivatePanel == null)
        {
            float value = armorSkill.skillValue[armorSkill.skillLvl];
            fillValue[1].text = value.ToString();
            
            if(value == 0)
            FillTheBar(fill[1], 0);
            else
                FillTheBar(fill[1], 100);
        }
        else
        {
            // FillTheBar(fill[1], hubManager.armorActivatePanel.value + (improvementScoreCounter[1] * pointValue));
            FillTheBar(fill[1], 100);
            fillValue[1].text = ((hubManager.armorActivatePanel.value) + armorSkill.skillValue[armorSkill.skillLvl]).ToString();
        }

        if (hubManager.weaponActivatePanel == null)
        {
            float value = damageSkill.skillValue[damageSkill.skillLvl];
            fillValue[2].text = value.ToString();

            if (value == 0)
                FillTheBar(fill[2], 0);
            else
                FillTheBar(fill[2], 100);
        }
        else
        {
            //FillTheBar(fill[2], hubManager.weaponActivatePanel.value + (improvementScoreCounter[2] * pointValue));
            FillTheBar(fill[2], 100);
            fillValue[2].text = ((hubManager.weaponActivatePanel.value) + damageSkill.skillValue[damageSkill.skillLvl]).ToString();
        }


        UpdateImprovementPoints();
    }

    public void PlaySelectedAnimation()
    {
        selectedParticle.Play();
    }

    public void UpdateImprovementPoints()
    {
        if (MoneyHandler.Instance == null)
            return;

        //if (MoneyHandler.Instance.currentImprovementPoints <= 0)
        //    return;

        //for (int i = 0; i < improvementScoreCounter.Length; i++)
        //{
        //    if (improvementScoreCounter[i] < maxCount)
        //        improvementButton[i].SetActive(true);
        //}
    }

    public void UseExperiencePoints(int number)
    {
        improvementScoreCounter[number] += 1;
        //MoneyHandler.Instance.currentImprovementPoints -= 1;

        // switch (number)
        // {
        //case 0:
        //    FillTheBar(fill[number], (improvementScoreCounter[number] * pointValue));
        //    break;

        //case 1:
        //    if (hubManager.armorActivatePanel == null)
        //        FillTheBar(fill[number], (improvementScoreCounter[number] * pointValue));
        //    else
        //        FillTheBar(fill[number], hubManager.armorActivatePanel.value + (improvementScoreCounter[number] * pointValue));
        //    break;

        //case 2:
        //    if (hubManager.weaponActivatePanel == null)
        //        FillTheBar(fill[number], (improvementScoreCounter[number] * pointValue));
        //    else
        //        FillTheBar(fill[number], hubManager.weaponActivatePanel.value + (improvementScoreCounter[number] * pointValue));
        //    break;
        // }

        if (improvementScoreCounter[number] >= 8)
            improvementButton[number].SetActive(false);

        //if (MoneyHandler.Instance.currentImprovementPoints <= 0)
        //    foreach (var button in improvementButton)
        //        button.SetActive(false);
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
        fillingLevel /= 100;
        DOTween.To(() => fillingImage.fillAmount, x => fillingImage.fillAmount = x, fillingLevel, fillingTime);
    }
}
