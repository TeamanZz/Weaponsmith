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

    public void OpenPanel(int panelIndex)
    {
        //Enchant panel
        if (panelIndex == 4)
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
            panels[4].SetActive(true);
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

                //disable blueprints notification
                if (i == 3)
                {
                    MarkNotificationHandler.Instance.blueprintsMark.SetActive(false);
                }

                if (i == 2 || i == 3)
                {
                    Camera.main.transform.localPosition = cameraTablePosition;
                    Camera.main.transform.localRotation = Quaternion.Euler(cameraTableRotation.x, cameraTableRotation.y, cameraTableRotation.z);
                    FurnaceLightController.Instance.lightCoefficient = 5;
                }
                else
                {
                    Camera.main.transform.localPosition = cameraRoomPosition;
                    Camera.main.transform.localRotation = Quaternion.Euler(cameraRoomRotation.x, cameraRoomRotation.y, cameraRoomRotation.z);
                    FurnaceLightController.Instance.lightCoefficient = 1;
                    CustomerController.Instance.ChangeAnimation();

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