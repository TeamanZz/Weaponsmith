using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelsHandler : MonoBehaviour
{
    public EnemyHealthBar enemyHealthBar;
    public static PanelsHandler Instance;
    public int dungeonNumber;
    public int dropChanceImprovements = 7;

    [SerializeField] private List<string> panelNames = new List<string>();
    [SerializeField] private List<GameObject> panels = new List<GameObject>();
    [SerializeField] private List<BottomButton> bottomButtons = new List<BottomButton>();
    [SerializeField] private TextMeshProUGUI panelName;
    [SerializeField] private GameObject commonElement;

    [Header("Sub panels in craft panel")]
    [SerializeField] private GameObject bodyContent;
    [SerializeField] private GameObject tipContent;
    [SerializeField] private TextMeshProUGUI craftPanelLabel;

    [SerializeField] private Vector3 cameraTablePosition;
    [SerializeField] private Vector3 cameraTableRotation;
    [SerializeField] private Vector3 cameraRoomPosition;
    [SerializeField] private Vector3 cameraRoomRotation;


    public GameObject mainCamera;
    public GameObject dungeonCamera;
    public static bool currentLocationInTheDungeon = false;


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
            if(enemyHealthBar.isInitialization == true)
                enemyHealthBar.OpenHealthBar();
                
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
            enemyHealthBar.ClosedHealthBar(); 

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

    public void SwitchCraftPanelContent()
    {
        if (bodyContent.activeSelf == true)
        {
            bodyContent.SetActive(false);
            tipContent.SetActive(true);
            craftPanelLabel.text = "Tip";
        }
        else
        {
            bodyContent.SetActive(true);
            tipContent.SetActive(false);
            craftPanelLabel.text = "Body";
        }
    }
}