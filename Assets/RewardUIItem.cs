using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardUIItem : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Image icon;
    public void InitializationPanel(Sprite sprite, string title)
    {
        icon.sprite = sprite;
        this.title.text = title;
    }
}
