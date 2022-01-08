using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FTUEManager : MonoBehaviour
{
    public static FTUEManager Instance;
    public bool FTUEEnabled;
    public ScrollRect upgradesScrollRect;
    public Button settingsButton;
    public List<GameObject> FTUEPanels = new List<GameObject>();
    public List<Button> bottomButtons = new List<Button>();
    public GameObject FTUEContentBlock;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        settingsButton.enabled = false;
        DisableBottomButtons();

        ChangeFTUEState(0);
    }

    public void ChangeFTUEState(int stateIndex)
    {
        if (!FTUEEnabled)
            return;

        if (stateIndex == 0)
        {
            ActivateFtuePanel(0);
            upgradesScrollRect.enabled = false;
            ChangeUpgradesButtonColor();
        }

        if (stateIndex == 1)
        {
            ActivateFtuePanel(1);
            FTUEContentBlock.SetActive(true);
            ActivateBottomButton(2);
        }

        if (stateIndex == 2)
        {
            ActivateFtuePanel(2);
            FTUEContentBlock.SetActive(false);
        }

    }

    private void ActivateFtuePanel(int index)
    {
        for (int i = 0; i < FTUEPanels.Count; i++)
        {
            if (i == index)
            {
                FTUEPanels[i].SetActive(true);
            }
            else
            {
                FTUEPanels[i].SetActive(false);
            }
        }
    }

    private void ChangeUpgradesButtonColor()
    {
        bottomButtons[1].GetComponent<BottomButton>().ChangeColorOnSelect();
    }

    private void ActivateBottomButton(int index)
    {
        for (int i = 0; i < bottomButtons.Count; i++)
        {
            if (i == index)
            {
                bottomButtons[i].enabled = true;
            }
            else
            {
                bottomButtons[i].enabled = false;
            }
        }
    }

    private void DisableBottomButtons()
    {
        for (int i = 0; i < bottomButtons.Count; i++)
        {
            bottomButtons[i].enabled = false;
        }
    }
}
