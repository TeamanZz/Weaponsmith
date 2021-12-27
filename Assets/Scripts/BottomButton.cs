using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomButton : MonoBehaviour
{
    private Color defaultColor;
    [SerializeField] private Color selectColor;
    private Image buttonImage;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        defaultColor = buttonImage.color;
    }

    public void ChangeColorOnSelect()
    {
        PanelsHandler.Instance.ResetButtonColorOnDeselect();
        buttonImage.color = selectColor;
    }

    public void ResetColor()
    {
        buttonImage.color = defaultColor;
    }
}