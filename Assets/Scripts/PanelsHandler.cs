using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelsHandler : MonoBehaviour
{
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

    [Header("Purchase setup")]
    public int purchaseCount;
    public List<int> purchaseReference = new List<int>();
    public int numberOfPurchases = 1;
    public TextMeshProUGUI purchaseText;

    public delegate void UpdatingAction();
     
    public event UpdatingAction updatingUiPanelsEvent;
    public void Awake()
    {
        Instance = this;
        dropChanceImprovements = 7;

        if (purchaseCount == 0)
            purchaseCount = 1;
        else
            purchaseCount = PlayerPrefs.GetInt("purchaseCount");

        Debug.Log(purchaseCount);

        if (purchaseReference.Count > 0)
        {
            numberOfPurchases = purchaseReference[purchaseCount - 1];
            purchaseText.text = purchaseReference[purchaseCount - 1].ToString();
        }
        currentLocationInTheDungeon = false;
        mainCamera.SetActive(true);
        dungeonCamera.SetActive(false);

        InitializationPurchase();
    }
    public void Start()
    {
        OpenPanel(1);
    }

    public void PurchaseQuantityRatio()
    {
        purchaseCount += 1;
        if(purchaseCount > purchaseReference.Count)
        {
            purchaseCount = 1;
        }
        PlayerPrefs.SetInt("purchaseCount", purchaseCount);

        if(purchaseReference.Count <= 0)
        {
            Debug.Log("purchaseReference == null");
            Debug.LogError("");
            return;
        }

        InitializationPurchase();
    }

    public void InitializationPurchase()
    {
        numberOfPurchases = purchaseReference[purchaseCount - 1];
        purchaseText.text = purchaseReference[purchaseCount - 1].ToString();

        updatingUiPanelsEvent?.Invoke();

    }
    public void OpenPanel(int panelIndex)
    {
        //dungeon panel
        if (panelIndex == dungeonNumber)
        {
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