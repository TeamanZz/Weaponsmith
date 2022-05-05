using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillItemView : MonoBehaviour
{
    [Header("Data")]
    public SkillItem itemData;

    [Header("View Settings")]
    public Image iconImage;
    public Image border;
    public TextMeshProUGUI title;
    public TextMeshProUGUI level;

    public Image blur;
    public void Initialization(SkillItem skillItem)
    {
        itemData = skillItem;
        UpdateView(itemData);
    }

    public void UpdateView(SkillItem item)
    {
        // iconImage.sprite = item.iconImage;
        // border.color = item.borderColor;
        title.text = item.skillName;
        level.text = item.skillLvl.ToString();
    }

    public void EnableBlur()
    {
        blur.gameObject.SetActive(true);
    }

    public void DisableBlur()
    {
        blur.gameObject.SetActive(false);
    }
}
