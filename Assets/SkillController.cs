using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class SkillController : MonoBehaviour
{
    public static SkillController skillController;
    public List<SkillItem> skills = new List<SkillItem>();
    public int minNumber = 6;
    public int maxNumber = 8;

    [Header("View Settings")]
    public GridLayoutGroup group;
    public SkillItemView emptyPrefab;
    public List<SkillItemView> skillItemViews = new List<SkillItemView>();

    public AnimationCurve curve;
    public void Awake()
    {
        skillController = this;
        Initialization();
    }

    public void Initialization()
    {
        skillItemViews.Clear();
        for (int i = 0; i < skills.Count; i++)
        {
            var newViewPanel = Instantiate(emptyPrefab, group.transform);
            newViewPanel.Initialization(skills[i]);
            skillItemViews.Add(newViewPanel);
        }

    }

    [ContextMenu("Test")]
    public void IncreaseSkillLevel()
    {
        int number = UnityEngine.Random.Range(minNumber, (int)curve.keys[curve.length - 1].time);//(int)curve.keys[curve.length - 1].time);
        Debug.Log(number + " Lenght =" + curve.keys[curve.length - 1].time);

        StartCoroutine(Bluring(number));
    }

    public IEnumerator Bluring(int value)
    {
        for (int i = 0; i < value; i++)
        {
            int itemNumber = UnityEngine.Random.Range(0, skills.Count - 1);
            skillItemViews[itemNumber].EnableBlur();
            Debug.Log(itemNumber + (float)curve.Evaluate(i));

            yield return new WaitForSeconds((float)curve.Evaluate(i));
            skillItemViews[itemNumber].DisableBlur();
        }
    }
}
