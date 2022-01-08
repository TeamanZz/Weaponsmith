using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FTUEManager : MonoBehaviour
{
    public static FTUEManager Instance;
    public bool FTUEEnabled;
    public ScrollRect upgradesScrollRect;
    public ScrollRect blueprintsScrollRect;
    public ScrollRect workshopScrollRect;
    public Button settingsButton;
    public List<GameObject> FTUEPanels = new List<GameObject>();
    public List<Button> bottomButtons = new List<Button>();
    public GameObject FTUEContentBlock;
    public GameObject FTUEContentBlockCraft;
    public int currentFTUEState;

    public SettingsManager settingsManager;

    public List<Button> craftArrows = new List<Button>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("FTUEEnabled") == "false")
        {
            FTUEEnabled = false;
        }
        if (PlayerPrefs.GetString("FTUEEnabled") == "")
        {
            if (PlayerPrefs.GetInt("MoneyPerSecond") > 1)
            {
                settingsManager.DeleteAllProgress();
            }
            FTUEEnabled = true;
        }

        if (!FTUEEnabled)
            return;
        settingsButton.enabled = false;
        DisableBottomButtons();

        ChangeFTUEState(0);
    }

    public void ChangeFTUEState(int stateIndex)
    {

        if (!FTUEEnabled)
            return;
        currentFTUEState = stateIndex;

        //click buy upgrade buttonc
        if (stateIndex == 0)
        {
            ActivateFtuePanel(0);
            upgradesScrollRect.enabled = false;
            ChangeUpgradesButtonColor();
            // FTUEContentBlockCraft.SetActive(true);
        }

        //click craft panel button
        if (stateIndex == 1)
        {
            ActivateFtuePanel(1);
            FTUEContentBlock.SetActive(true);
            ActivateBottomButton(2);
        }

        //click wooden sword body button
        if (stateIndex == 2)
        {
            ActivateFtuePanel(2);
            FTUEContentBlock.SetActive(false);
            craftArrows[0].enabled = false;
            craftArrows[1].enabled = false;
            DisableBottomButtons();
        }

        //click right arrow button in craft panel 
        if (stateIndex == 3)
        {
            ActivateFtuePanel(3);
            craftArrows[1].enabled = true;
            FTUEContentBlockCraft.SetActive(true);
            // FTUEContentBlock.SetActive(false);
        }

        //click wooden sword tip button
        if (stateIndex == 4)
        {
            ActivateFtuePanel(2);
            craftArrows[1].enabled = false;
            FTUEContentBlockCraft.SetActive(false);
            // FTUEContentBlock.SetActive(false);
        }

        //merge items
        if (stateIndex == 5)
        {
            ActivateFtuePanel(4);
            // ActivateBottomButton(3);
            FTUEContentBlock.SetActive(true);
        }

        //sell item
        if (stateIndex == 6)
        {

        }

        //click blueprints panel
        if (stateIndex == 7)
        {
            ActivateFtuePanel(5);
            ActivateBottomButton(3);
        }

        //buy axe blueprint
        if (stateIndex == 8)
        {
            ActivateFtuePanel(0);
            FTUEContentBlock.SetActive(false);
            blueprintsScrollRect.enabled = false;
            DisableBottomButtons();

            // ActivateBottomButton(3);
        }

        //click workshop panel
        if (stateIndex == 9)
        {
            ActivateFtuePanel(6);
            FTUEContentBlock.SetActive(true);
            ActivateBottomButton(0);
        }

        //Buy workshop item
        if (stateIndex == 10)
        {
            ActivateFtuePanel(0);
            FTUEContentBlock.SetActive(false);
            workshopScrollRect.enabled = false;
            DisableBottomButtons();
        }

        if (stateIndex == 11)
        {
            settingsButton.enabled = true;
            FTUEContentBlock.SetActive(false);
            blueprintsScrollRect.enabled = true;
            upgradesScrollRect.enabled = true;
            workshopScrollRect.enabled = true;

            craftArrows[0].enabled = true;
            craftArrows[1].enabled = true;

            EnableBottomButtons();
            FTUEPanels[0].SetActive(false);

            FTUEEnabled = false;
            PlayerPrefs.SetString("FTUEEnabled", "false");
        }
    }

    private void ActivateFtuePanel(int index)
    {
        if (!FTUEEnabled)
            return;
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
        if (!FTUEEnabled)
            return;
        bottomButtons[1].GetComponent<BottomButton>().ChangeColorOnSelect();
    }

    private void ActivateBottomButton(int index)
    {
        if (!FTUEEnabled)
            return;
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

    public void DisableBottomButtons()
    {
        if (!FTUEEnabled)
            return;
        for (int i = 0; i < bottomButtons.Count; i++)
        {
            bottomButtons[i].enabled = false;
        }
    }

    public void EnableBottomButtons()
    {
        for (int i = 0; i < bottomButtons.Count; i++)
        {
            bottomButtons[i].enabled = true;
        }
    }
}
