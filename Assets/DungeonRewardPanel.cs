using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DungeonRewardPanel : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public string[] resultInTitle;

    public Image[] starsViewImage;
    public void Initialization(bool isWon, int starsValue)
    {
        switch(isWon)
        {
            case true:
                titleText.text = resultInTitle[0]; 
                break;

            case false:
                titleText.text = resultInTitle[0];
                break;
        }

        StartCoroutine(StarsInitialization(3));
    }

    public IEnumerator StarsInitialization(int number)
    {
        yield return new WaitForSeconds(5);
    }
}
