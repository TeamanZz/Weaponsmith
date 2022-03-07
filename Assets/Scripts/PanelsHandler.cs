using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelsHandler : MonoBehaviour
{
    public DungeonBuilder dungeonBuilder;
    public static PanelsHandler Instance;
    public int dungeonNumber;
    public int dropChanceImprovements = 7;

    [SerializeField] private List<string> panelNames = new List<string>();
    [SerializeField] private List<GameObject> panels = new List<GameObject>();
    [SerializeField] private List<BottomButton> bottomButtons = new List<BottomButton>();
    [SerializeField] private TextMeshProUGUI panelName;
    [SerializeField] private GameObject commonElement;

    public GameObject mainCamera;
    public GameObject dungeonCamera;
    public static bool currentLocationInTheDungeon = false;

    [Header("Dungeon panel")]
    public Button dungeonBlockButton;
    public Button enchantmentBlockButton;

    public Image dungeonBlockButtonIcon;
    public Image enchantmentBlockButtonIcon;
    public void Awake()
    {
        Instance = this;
        dropChanceImprovements = 7;

        currentLocationInTheDungeon = false;
        mainCamera.SetActive(true);
        dungeonCamera.SetActive(false);
    }

    public void Start()
    {
        EnabledDungeonButton();
        EnabledEnchantmentButton();
        OpenPanel(1);
    }
    public void Initialization(PanelsStortage stortage)
    {
        panels.Clear();
        panels.AddRange(stortage.panels);
        //OpenPanel(1);
    }

    public void OpenPanel(int panelIndex)
    {
        //dungeon panel
        if (panelIndex == dungeonNumber)
        {
            if (WorkshopItem.dungeoonIsOpen == 0)
            {
                dungeonBlockButton.interactable = false;

                currentLocationInTheDungeon = true;
                mainCamera.SetActive(false);
                dungeonCamera.SetActive(true);

                commonElement.SetActive(false);
                panels[panelIndex].SetActive(false);
                return;
            }
            else
                dungeonBlockButton.interactable = true;

            currentLocationInTheDungeon = true;
            mainCamera.SetActive(false);
            dungeonCamera.SetActive(true);

            for (int i = 0; i < panels.Count; i++)
            {
                panels[i].SetActive(false);
            }

            commonElement.SetActive(true);
            panels[panelIndex].SetActive(true);
            return;
        }
        else
        {

            currentLocationInTheDungeon = false;
            mainCamera.SetActive(true);
            dungeonCamera.SetActive(false);
        }

        for (int i = 0; i < panels.Count; i++)
        {
            if (i == panelIndex)
            {
                panels[i].SetActive(true);
            }
            else
            {
                panels[i].SetActive(false);
            }
        }
        commonElement.SetActive(true);
    }

    public void EnabledDungeonButton()
    {
        if (PlayerPrefs.GetInt("dungeoonIsOpen") == 0)
        {
            dungeonBlockButton.interactable = false;
            dungeonBlockButtonIcon.color = Color.grey;
        }
        else
        {
            dungeonBlockButton.interactable = true;
            dungeonBlockButtonIcon.color = Color.white;
        }
    }
    public void EnabledEnchantmentButton()
    {
        if (PlayerPrefs.GetInt("enchantmentIsOpen") == 0)
        {
            enchantmentBlockButton.interactable = false;
            enchantmentBlockButtonIcon.color = Color.gray;
        }
        else
        {
            enchantmentBlockButton.interactable = true;
            enchantmentBlockButtonIcon.color = Color.white;
        }
    }
    public void ResetButtonColorOnDeselect()
    {
        for (int i = 0; i < bottomButtons.Count; i++)
        {
            bottomButtons[i].ResetColor();
        }
    }

    public void ShowPanelName(int panelIndex)
    {
        panelName.text = panelNames[panelIndex];
    }
}