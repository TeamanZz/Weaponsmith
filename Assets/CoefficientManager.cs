using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoefficientManager : MonoBehaviour
{
    [Header("Custom Toggle")]
    public TextMeshProUGUI toggleCoefficientText;
    public int[] toggleCoefficient = { 1, 10, 25 };
    [SerializeField] private int currentCoefNumber = 0;

    public void Awake()
    {
        InitializationUI();
    }
    public void InitializationUI()
    {
        toggleCoefficientText.text = toggleCoefficient[currentCoefNumber].ToString() + "x";
    }

    public void ChangeCoefficient()
    {
        if (currentCoefNumber < toggleCoefficient.Length - 1)
            currentCoefNumber += 1;
        else
            currentCoefNumber = 0;

        InitializationUI();
    }
}
