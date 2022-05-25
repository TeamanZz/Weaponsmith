using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FTUESystem : MonoBehaviour
{
    public Button mainButton;
    public List<Image> blurPanels = new List<Image>();

    public bool isTraining;

    public void Awake()
    {
        LoadData();    
    }

    bool IntToBool(int n)
   => n == 1;
    int BoolToInt(bool b)
        => (b ? 1 : 0);

    public void SaveData()
    {
        PlayerPrefs.SetInt($"FTUESystem{0}isTraining", BoolToInt(isTraining));
        
    }

    public void LoadData()
    {

    }

    public void RemoveData()
    {

    }


}
