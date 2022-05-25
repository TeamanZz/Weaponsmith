using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FTUESystem : MonoBehaviour
{
    public static FTUESystem trainingSystem;
    public MoneyHandler moneyHandler;

    [Header("View Settings")]
    public Transform ftueTransform;

    public Button mainButton;
    public Image mainTitlePanel;

    public TextMeshProUGUI mainTitles;
    public List<ViewUI> uiSettings = new List<ViewUI>();

    [Header("DATA Settings")]
    public bool isTrainingCompleted;
    public int currentTrainingNumber;

    [SerializeField] private ViewUI currentUISettings;

    public void Awake()
    {
        trainingSystem = this;

        mainButton.gameObject.SetActive(false);
        mainTitlePanel.gameObject.SetActive(false);

        DisableAllSettings();

        LoadData();
    }

    public void DisableAllSettings()
    {
        foreach (var item in uiSettings)
            item.group.SetActive(false);

    }

    bool IntToBool(int n)
        => n == 1;
    int BoolToInt(bool b)
        => (b ? 1 : 0);

    public void SaveData()
    {
        PlayerPrefs.SetInt($"FTUESystem{0}isTraining", BoolToInt(isTrainingCompleted));
        PlayerPrefs.SetInt($"FTUESystem{0}trainingNumber", currentTrainingNumber);
    }

    public void LoadData()
    {
        isTrainingCompleted = IntToBool(PlayerPrefs.GetInt($"FTUESystem{0}isTraining"));
        currentTrainingNumber = PlayerPrefs.GetInt($"FTUESystem{0}trainingNumber");

        if (isTrainingCompleted == true)
            return;

        CheckTrainingState();
    }

    [ContextMenu("Remove Data")]
    public void RemoveData()
    {
        PlayerPrefs.SetInt($"FTUESystem{0}isTraining", BoolToInt(false));
        PlayerPrefs.SetInt($"FTUESystem{0}trainingNumber", 0);
        Awake();
    }

    public void FirstInitialization()
    {
        moneyHandler.moneyPerSecond = 0;
        moneyHandler.moneyCount = 0;
    }

    //  awake init instruction
    public void CheckTrainingState()
    {
        DisableAllSettings();

        ViewUI viewUI = uiSettings[currentTrainingNumber];
        viewUI.group.SetActive(true);

        if (viewUI.buttonPositions != null)
        {
            mainButton.gameObject.SetActive(true);
            mainButton.image.rectTransform.sizeDelta = viewUI.buttonRectTransform;
            mainButton.transform.position = viewUI.buttonPositions.position;

            if (viewUI.customInstruction == false)
            {
                mainButton.onClick.RemoveAllListeners();
                mainButton.onClick.AddListener(() => ChangeState());
            }

        }
        else
            mainButton.gameObject.SetActive(false);

        if (viewUI.titlesText != " ")
        {
            mainTitlePanel.gameObject.SetActive(true);
            mainTitles.text = viewUI.titlesText;
        }
        else
            mainTitlePanel.gameObject.SetActive(false);

        currentUISettings = viewUI;
        CheckCurrentInstruction();
    }

    [Space(10)]
    [Header("Instruction 1")]
    [SerializeField] private int moneyPerSec = 10;
    [SerializeField] private int moneyPurpose = 1500;

    [Space(10)]
    [Header("Instruction 2")]
    [SerializeField] private ScrollRect scrollGroup;
    [SerializeField] private PanelItem firstItem;

    [Space(10)]
    [Header("Instruction 3")]
    [SerializeField] private PanelsHandler panelsHandler;
    [SerializeField] private Image armorButton;
    [SerializeField] private TextMeshProUGUI armorText;

    [Space(10)]
    [Header("Instruction 3")]
    [SerializeField] private DungeonHubManager dungeonHubManager;

    //  start init instruction
    public void CheckCurrentInstruction()
    {
        switch (currentUISettings.instructionNumber)
        {
            case 0:
                break;

            case 1:
                moneyHandler.moneyPerSecond = moneyPerSec;
                break;

        }
    }

    public void Update()
    {
        if (currentUISettings == null)
            return;

        switch (currentUISettings.instructionNumber)
        {
            case 0:
                break;

            case 1:
                scrollGroup.vertical = false;
                if (moneyHandler.moneyCount >= moneyPurpose)
                {
                    scrollGroup.vertical = false;
                    ChangeState();
                }
                break;

            case 2:
                scrollGroup.vertical = false;
                if (firstItem.currentState == PanelItemState.Collapsed)
                {
                    scrollGroup.vertical = false;
                    ChangeState();
                }
                break;

            case 3:
                armorButton.gameObject.SetActive(false); 
                armorText.gameObject.SetActive(false);
                
                if (panelsHandler.currentIndex == 2)
                    ChangeState();
                break;

            case 4:
                if (panelsHandler.currentIndex != 2)
                    panelsHandler.OpenPanel(2);

                armorText.gameObject.SetActive(false);
                armorButton.gameObject.SetActive(false);
                
                if (dungeonHubManager.weaponActivatePanel != null)
                    ChangeState();
                break;
        }
    }

    //  exit metod
    public void ChangeState()
    {
        currentTrainingNumber += 1;
        if (currentTrainingNumber > uiSettings.Count - 1)
        {
            DisableAllSettings();
            Debug.Log("Training Is Lost");
            currentUISettings = null;

            mainButton.gameObject.SetActive(false);
            mainTitlePanel.gameObject.SetActive(false);

            scrollGroup.vertical = true;

            armorButton.gameObject.SetActive(true);
            armorText.gameObject.SetActive(true);

            isTrainingCompleted = true;
            SaveData();
            return;
        }

        SaveData();
        CheckTrainingState();
    }


}

[Serializable]
public class ViewUI
{
    [Header("Settings")]
    public GameObject group;
    public string titlesText;

    [Header("Logics Settings")]
    public bool customInstruction;
    public int instructionNumber;

    [Header("View Setting")]
    public RectTransform buttonPositions;
    public Vector2 buttonRectTransform;
    //public Image blurPanel;
}