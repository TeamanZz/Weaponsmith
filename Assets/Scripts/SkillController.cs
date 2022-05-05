using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

[Serializable]
public class SkillController : MonoBehaviour
{
    public static SkillController skillController;
    public PreviewSkillWindow previewSkillWindow;
    public GameObject previewWindow;
    public Coroutine currentCorutine;

    public List<SkillItem> skills = new List<SkillItem>();
    [SerializeField] private List<SkillItem> enableSkills = new List<SkillItem>();

    public int minNumber = 6;
    public int maxNumber = 8;

    [Header("View Settings")]
    public GridLayoutGroup group;
    public SkillItemView emptyPrefab;
    public List<SkillItemView> skillItemViews = new List<SkillItemView>();

    public AnimationCurve curve;

    [Header("Button Settings")]
    public Button buyButton;
    public TextMeshProUGUI buttonText;

    [Header("improvement points")]
    public int maxPoints = 24;
    public int improvementCount = 0;
    public int currentImprovementPoints = 0;
    public void Awake()
    {
        skillController = this;
        previewWindow.SetActive(false);

        enableSkills.Clear();
        enableSkills.AddRange(skills);

        Initialization();
        CheckButtonState();

        int currentMax = 0;
        foreach (var item in skillItemViews)
        {
            currentMax += item.itemData.skillValue.Count;
        }
        maxPoints = currentMax;
    }

    public void Initialization()
    {
        skillItemViews.Clear();
        for (int i = 0; i < enableSkills.Count; i++)
        {
            var newViewPanel = group.transform.GetChild(i).GetComponent<SkillItemView>();
            newViewPanel.Initialization(enableSkills[i]);
            skillItemViews.Add(newViewPanel);
        }
        CheckSkillState();
    }

    [ContextMenu("Add Point")]
    public void AddImprovementPoint()
    {
        if (improvementCount >= maxPoints)
            return;

        currentImprovementPoints += 1;
        CheckButtonState();
        CheckSkillState();
        //LoadoutController.Instance.UpdateImprovementPoints();
    }

    public void CheckButtonState()
    {
        if (currentImprovementPoints == 0 || currentCorutine != null || enableSkills.Count == 0)
            buyButton.interactable = false;
        else
            buyButton.interactable = true;

        buttonText.text = currentImprovementPoints + " x";
    }

    public void CheckUpgradePoints()
    {
        if (currentImprovementPoints > 0)
        {
            currentImprovementPoints -= 1;
            CheckButtonState();
        }
        else
            return;

        IncreaseSkillLevel();
    }

    [ContextMenu("Test")]
    public void IncreaseSkillLevel()
    {
        int number = UnityEngine.Random.Range(minNumber, (int)curve.keys[curve.length - 1].time);//(int)curve.keys[curve.length - 1].time);
        Debug.Log(number + " Lenght =" + curve.keys[curve.length - 1].time);

        currentCorutine = StartCoroutine(Bluring(number));
    }

    public void CheckSkillState()
    {
        foreach (var item in skillItemViews)
        {
            if (item.itemData.skillLvl >= item.itemData.skillValue.Count)
            {
                item.level.text = "Max Lvl";
                enableSkills.Remove(item.itemData);
                skillItemViews.Remove(item);
            }
            else
                item.level.text = item.itemData.skillLvl.ToString();
        }
    }

    public IEnumerator Bluring(int value)
    {
        buyButton.interactable = false;

        for (int i = 0; i < value; i++)
        {
            int itemNumber = UnityEngine.Random.Range(0, enableSkills.Count);
            skillItemViews[itemNumber].EnableBlur();
            Debug.Log("Number - " + itemNumber + " Lenght - " + (float)curve.Evaluate(i));

            yield return new WaitForSeconds((float)curve.Evaluate(i));
            skillItemViews[itemNumber].DisableBlur();
        }

        currentCorutine = StartCoroutine(LastBlur((float)curve.Evaluate(value)));
        //int endNumber = UnityEngine.Random.Range(0, enableSkills.Count);
        //skillItemViews[endNumber].itemData.skillLvl += 1;

        //ChechButtonState(); 
        //CheckSkillState();

        //buyButton.interactable = true;
    }

    public IEnumerator LastBlur(float time)
    {
        int endNumber = UnityEngine.Random.Range(0, enableSkills.Count);
        skillItemViews[endNumber].itemData.skillLvl += 1;
        skillItemViews[endNumber].EnableBlur();

        yield return new WaitForSeconds(time);
        previewWindow.SetActive(true);
        previewSkillWindow.ItemInitialization(skillItemViews[endNumber].itemData);

        skillItemViews[endNumber].DisableBlur();
        currentCorutine = null;

        CheckButtonState();
        CheckSkillState();
    }
}
