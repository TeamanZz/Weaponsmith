using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreviewSkillWindow : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI title;
    public TextMeshProUGUI lastLevel;
    public TextMeshProUGUI nextLevel;
    public Image borderImage;

    public void ItemInitialization(SkillItem newItem)
    {
        iconImage.sprite = newItem.iconImage;
        iconImage.SetNativeSize();
        borderImage.color = newItem.borderColor;

        title.text = newItem.skillName;
        lastLevel.text = "Lvl " + (newItem.skillLvl - 1).ToString();

        if (newItem.skillLvl >= newItem.skillValue.Count - 1)
        {
            nextLevel.text = "Max Lvl";
        }
        else
            nextLevel.text = "Lvl " + newItem.skillLvl.ToString();
    }

    public void ClosedWindow()
    {
        gameObject.SetActive(false);
    }
}
