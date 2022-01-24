using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelsHandler : MonoBehaviour
{
    public static PanelsHandler Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private List<string> panelNames = new List<string>();
    [SerializeField] private List<GameObject> panels = new List<GameObject>();
    [SerializeField] private List<BottomButton> bottomButtons = new List<BottomButton>();
    [SerializeField] private TextMeshProUGUI panelName;
    [SerializeField] private GameObject commonElement;
    [SerializeField] private List<GameObject> enchantPanelEffects = new List<GameObject>();

    [Header("Sub panels in craft panel")]
    [SerializeField] private GameObject bodyContent;
    [SerializeField] private GameObject tipContent;
    [SerializeField] private TextMeshProUGUI craftPanelLabel;

    [SerializeField] private Vector3 cameraTablePosition;
    [SerializeField] private Vector3 cameraTableRotation;
    [SerializeField] private Vector3 cameraRoomPosition;
    [SerializeField] private Vector3 cameraRoomRotation;

    public void Start()
    {
        OpenPanel(1);
    }
    public void OpenPanel(int panelIndex)
    {
        //Enchant panel
        if (panelIndex == 3)
        {
            for (int i = 0; i < 4; i++)
            {
                panels[i].SetActive(false);
            }

            for (int i = 0; i < enchantPanelEffects.Count; i++)
            {
                enchantPanelEffects[i].SetActive(true);
            }

            commonElement.SetActive(false);
            panels[3].SetActive(true);
            return;
        }

        for (int i = 0; i < panels.Count; i++)
        {
            if (i == panelIndex)
            {
                panels[i].SetActive(true);

                //disable workshop notification
                if (i == 0)
                {
                    MarkNotificationHandler.Instance.workshopMark.SetActive(false);
                }

            }
            else
            {
                panels[i].SetActive(false);
            }
        }


        commonElement.SetActive(true);
        for (int i = 0; i < enchantPanelEffects.Count; i++)
        {
            enchantPanelEffects[i].SetActive(false);
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