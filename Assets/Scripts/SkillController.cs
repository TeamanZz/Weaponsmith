using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using DG.Tweening;

[Serializable]
public class SkillController : MonoBehaviour
{
    public static SkillController skillController;
    public PreviewSkillWindow previewSkillWindow;
    public GameObject previewWindow;

    public List<SkillItem> mainSkillsData = new List<SkillItem>();
    [SerializeField] private List<SkillItemView> mainSkillsView = new List<SkillItemView>();

    [Header("View Settings")]
    public GridLayoutGroup group;
    public SkillItemView emptyPrefab;

    public List<SkillItem> copyMainSkillsData = new List<SkillItem>();
    public List<SkillItemView> copyItemPanelsView = new List<SkillItemView>();

    [Header("Button Settings")]
    public Button buyButton;
    public TextMeshProUGUI buttonText;

    [Header("Improvement points")]
    public int maxPoints = 24;
    public int improvementCount = 0;
    public int currentImprovementPoints = 0;

    [Header("View Scills Settings")]
    public SkillItemView selectedPanel;

    [Header("New Skills Settings")]
    public int startNumber = 2;
    public int currentItemCount = 0;
    //public TextMeshProUGUI selectedDebuger;

    public void Awake()
    {
        foreach (var skill in mainSkillsData)
            skill.skillLvl = 0;

        skillController = this;
        previewWindow.SetActive(false);

        copyMainSkillsData.Clear();
        copyMainSkillsData.AddRange(mainSkillsData);

        foreach (var item in mainSkillsView)
        {
            LoadData(item);
        }
        
        int currentMax = 0;
        foreach (var item in mainSkillsData)
        {
            currentMax += (item.skillValue.Count - 1) - item.skillLvl;
        }
        maxPoints = currentMax;

        UpdateButtonUI();
    }

    bool IntToBool(int n)
        => n == 1;
    int BoolToInt(bool b)
        => (b ? 1 : 0);

    public void SaveData(SkillItem skillItem)
    {
        PlayerPrefs.SetInt($"SkillItem{skillItem.ID}SkillLvl", skillItem.skillLvl);
        PlayerPrefs.SetInt($"SkillItem{skillItem.ID}IsOpen", BoolToInt(skillItem.isActive));
        PlayerPrefs.SetInt($"CurrentImprovementPoints", currentImprovementPoints);
    }

    public void LoadData(SkillItemView skillItem)
    {
        int number = PlayerPrefs.GetInt($"SkillItem{skillItem.itemData.ID}SkillLvl");
        skillItem.itemData.skillLvl = number;
        bool isOpen = IntToBool(PlayerPrefs.GetInt($"SkillItem{skillItem.itemData.ID}IsOpen"));
        currentImprovementPoints = PlayerPrefs.GetInt($"CurrentImprovementPoints");

        if (skillItem.itemData.ID < startNumber)
            isOpen = true;

        if (number < skillItem.itemData.skillValue.Count - 1)
        {
            if (isOpen)
                OpenNewSkill();
            //skillItem.DisabledBlur();
            else
                skillItem.EnabledBlur();
        }
        else
        {
            CheckMaximalSkillState(skillItem);
            Debug.Log("Collapse Id =" + skillItem.itemData.ID);
        }
    }

    [ContextMenu("Remove Data")]
    public void RemoveData()
    {
        foreach (var item in mainSkillsData)
        {
            PlayerPrefs.SetInt($"SkillItem{item.ID}SkillLvl", 0);
            PlayerPrefs.SetInt($"SkillItem{item.ID}IsOpen", BoolToInt(false));
            PlayerPrefs.SetInt($"CurrentImprovementPoints", 0);
            Debug.Log("Skill ID = " + item.ID + " | Skill Lvl =" + item.skillLvl + " | State =" + item.isActive);
        }
    }
    public void Initialization(int firstNumber)
    {
        copyItemPanelsView.Clear();

        for (int i = 0; i < firstNumber; i++)
        {
            OpenNewSkill();
        }
        UpdateButtonUI();
    }

    [ContextMenu("Open Panel")]
    public void OpenNewSkill()
    {
        if (currentItemCount >= mainSkillsData.Count)
            return;

        var newViewPanel = group.transform.GetChild(currentItemCount).GetComponent<SkillItemView>();
        newViewPanel.Initialization(copyMainSkillsData[currentItemCount]);
        newViewPanel.DisabledBlur();
        newViewPanel.itemData.isActive = true;
        SaveData(newViewPanel.itemData);

        copyItemPanelsView.Add(newViewPanel);

        currentItemCount += 1;
    }

    [ContextMenu("Add Point")]
    public void AddImprovementPoint()
    {
        if (improvementCount >= maxPoints)
            return;

        currentImprovementPoints += 1;
        UpdateButtonUI();
    }

    public void UpdateButtonUI()
    {
        currentImprovementPoints = Mathf.Clamp(currentImprovementPoints, 0, maxPoints);

        if (currentImprovementPoints == 0 || selectedPanel == null || selectedPanel.itemData.skillLvl >= selectedPanel.itemData.skillValue.Count || copyMainSkillsData.Count == 0)
            buyButton.interactable = false;
        else
            buyButton.interactable = true;

        //if (selectedPanel != null)
        //    selectedDebuger.text = selectedPanel.itemData.skillName;
        //else
        //    selectedDebuger.text = "None";

        buttonText.text = currentImprovementPoints + " x";
    }

    //  upgrade button
    public void UpgradeSkill()
    {
        Debug.Log("Upgrade " + " | CurrentImprovementPoints = " + currentImprovementPoints + " | Selected Panel = " + selectedPanel + " | Max Points = " + maxPoints);
        if (currentImprovementPoints > 0 && selectedPanel != null && maxPoints > 0)
        {
            improvementCount += 1;
            Debug.Log("Upgrade True");
            maxPoints -= 1;
            currentImprovementPoints -= 1;
            selectedPanel.itemData.skillLvl += 1;

            //SaveData(selectedPanel.itemData);

            previewSkillWindow.gameObject.SetActive(true);
            previewSkillWindow.ItemInitialization(selectedPanel.itemData);

            UpdateButtonUI();
            CheckMaximalSkillState(selectedPanel);
        }
        else
            return;
    }

    public void CheckMaximalSkillState(SkillItemView item)
    {
        if (item.itemData.skillLvl == item.itemData.skillValue.Count - 1)
        {
            item.DisabledButton();
            item.itemData.isActive = false;
            item.level.text = "Max Lvl";
        }
        else
            item.level.text = item.itemData.skillLvl.ToString();

        SaveData(item.itemData);
    }

    public void DeselectedAllPanelsBorder()
    {
        Debug.Log("Deselected All");
        foreach (var panel in copyItemPanelsView)
        {
            panel.DeselectedBorder();
        }
        selectedPanel = null;

        UpdateButtonUI();
    }
}
