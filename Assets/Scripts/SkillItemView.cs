using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillItemView : MonoBehaviour
{
    [Header("Data")]
    public SkillController skillController;
    public SkillItem itemData;

    [Header("View Settings")]
    public Image selectedOutline;
    public Image blurPanel;

    public TextMeshProUGUI title;
    public TextMeshProUGUI level;
    public GameObject levelGreenIcon;

    public Button button;

    public bool borderIsActive = false;
    public bool panelIsActive = false;

    public void Initialization(SkillItem skillItem)
    {
        skillController = SkillController.skillController;
        itemData = skillItem;
        UpdateView(itemData);
    }

    public void UpdateView(SkillItem item)
    {
        title.text = item.skillName;
        level.text = item.skillLvl.ToString();
    }

    [ContextMenu("Check Border")]
    public void CheckButton()
    {
        if (!panelIsActive)
            return;

        if (skillController.selectedPanel != null)
        {
            if (skillController.selectedPanel != this)
                skillController.selectedPanel.DeselectedBorder();
            else
            {
                DeselectedBorder();
                return;
            }
        }
        SelectedBorder();

    }

    public void SelectedBorder()
    {
        if (!panelIsActive)
            return;

        skillController.selectedPanel = this;

        selectedOutline.gameObject.SetActive(true);

        skillController.UpdateButtonUI();
    }

    public void DeselectedBorder()
    {
        Debug.Log("Deselect Border");
        selectedOutline.gameObject.SetActive(false);

        skillController.selectedPanel = null;
        skillController.UpdateButtonUI();
    }

    public void EnabledBlur()
    {
        panelIsActive = false;
        blurPanel.gameObject.SetActive(true);
        levelGreenIcon.SetActive(false);
    }

    public void DisabledBlur()
    {
        panelIsActive = true;
        blurPanel.gameObject.SetActive(false);
        button.gameObject.SetActive(true);
        levelGreenIcon.SetActive(true);
    }

    public void DisabledButton()
    {
        Debug.Log("Diselected Button");
        button.gameObject.SetActive(false);

        borderIsActive = false;
        DeselectedBorder();
    }
}
