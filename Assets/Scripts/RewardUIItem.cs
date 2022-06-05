using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardUIItem : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Image icon;
    public void InitializePanel(Sprite sprite, string title)
    {
        icon.sprite = sprite;
        this.title.text = title;
    }
}
