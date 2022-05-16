using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class PanelsHandler : MonoBehaviour
{
    public static PanelsHandler Instance;

    public int dropChanceImprovements = 7;

    [Header("Panel Label")]
    [SerializeField] private TextMeshProUGUI panelNameLabel;
    [SerializeField] private GameObject panelLabelObject;

    [Header("Unlockable Buttons")]
    public Button dungeonButtonComponent;
    public Button boostersButtonComponent;
    public Image dungeonButtonImageComponent;
    public Image boostersButtonImageComponent;

    [Header("Camera's")]
    public GameObject mainCamera;
    public GameObject dungeonCamera;

    [Space]
    public GameObject dungeonPreviewUI;
    public GameObject dungeonBarsGroup;
    public GameObject dungeonEnteringPanel;
    public GameObject panelsContainer;

    public DungeonRewardPanel dungeonRewardPanel;
    public static bool currentLocationInTheDungeon = false;
    public GameObject loadoutObject;

    [Space]
    [SerializeField] private List<string> panelNames = new List<string>();
    [SerializeField] private List<GameObject> panels = new List<GameObject>();
    [SerializeField] private List<BottomButton> bottomButtons = new List<BottomButton>();

    [Header("Cool Down Settings")]
    public Button buttonToDungeon;
    public TextMeshProUGUI coolDownText;
    public List<GameObject> buttonsToDungeonGroup = new List<GameObject>();
    public Coroutine coolDownCoroutine;

    public bool timerRunning = false;

    public float reloadTime = 60;
    [SerializeField] private float currentTime;

    public GameObject anvilSound;
    public GameObject tapArea;

    public int currentIndex = 0;
    public void Awake()
    {
        Instance = this;

        currentLocationInTheDungeon = false;
        mainCamera.SetActive(true);
        dungeonCamera.SetActive(false);
        loadoutObject.SetActive(false);

        DisableDungeonPreviewUI();
    }

    public void Start()
    {
        WorkshopItem.dungeoonIsOpen = 1;
        dungeonButtonComponent.interactable = true;
        EnableDungeonButton();
        EnableBoostersButton();
        OpenPanel(1);
    }

    public void Update()
    {
        if (coolDownCoroutine == null && Input.GetKeyUp(KeyCode.Q))
            StartCoolDown();
    }

    public void Initialization(PanelsStortage stortage)
    {
        panels.Clear();
        panels.AddRange(stortage.panels);
    }

    public void HandleDungeonPanelUI()
    {
        if (DungeonManager.Instance.isDungeonStarted)
        {
            panelsContainer.SetActive(false);
            dungeonBarsGroup.SetActive(false);
        }
        else
        {
            EnablePanelsContainer();
            dungeonPreviewUI.SetActive(true);
            dungeonBarsGroup.SetActive(true);
        }
    }

    public void DisablePanelsContainer()
    {
        if (DungeonManager.Instance.isDungeonStarted)
        {
            panelsContainer.SetActive(false);
            dungeonBarsGroup.SetActive(false);
        }
    }

    public void EnablePanelsContainer()
    {
        panelsContainer.SetActive(true);
    }

    public void DisableDungeonPreviewUI()
    {
        dungeonPreviewUI.SetActive(false);
    }

    public void EnableDungeonEnteringPanel()
    {
        dungeonEnteringPanel.SetActive(true);
        loadoutObject.SetActive(false);
        dungeonEnteringPanel.transform.localScale = Vector3.one;
        StartCoroutine(IEEnableDungeonEnteringPanel());
    }

    public void DisableDungeonEnteringPanel()
    {
        dungeonEnteringPanel.transform.DOScale(0, 1f).SetEase(Ease.InBack);
    }

    private IEnumerator IEEnableDungeonEnteringPanel()
    {
        DungeonManager.Instance.SetCharacterOnStart();
        DisablePanelsContainer();
        yield return new WaitForSeconds(1);
        DisableDungeonEnteringPanel();
        DungeonCharacter.Instance.animator.SetBool("IsDungeonStarted", true);
        DungeonCharacter.Instance.EnableCharacterRun();
        SceneObjectsOptimizeHandler.Instance.EnableFirstPieces();
        SceneObjectsOptimizeHandler.Instance.DisablePreviewScene();
        yield return new WaitForSeconds(1);
        dungeonEnteringPanel.SetActive(false);
    }

    public void OpenPanel(int panelIndex)
    {
        if (panelIndex == currentIndex || panelIndex < 0 || panelIndex > panels.Count)
        {
            Debug.Log("Current = " + currentIndex + " | Panel index = " + panelIndex);
            return;
        }

        if (SkillController.skillController != null)
            if (panelIndex != 3 && SkillController.skillController.selectedPanel != null)
                SkillController.skillController.DeselectedAllPanelsBorder();

        currentIndex = panelIndex;
        //dungeon panel
        if (panelIndex == 2)
        {
            if (LoadoutController.Instance != null)
                LoadoutController.Instance.Initialization();

            anvilSound.SetActive(false);
            tapArea.SetActive(false);

            if (WorkshopItem.dungeoonIsOpen == 0)
            {
                Debug.Log("ISOPEN 0");
                currentLocationInTheDungeon = true;
                mainCamera.SetActive(false);
                dungeonCamera.SetActive(true);

                panelLabelObject.SetActive(false);
                panels[panelIndex].SetActive(false);


                return;
            }
            else
                dungeonButtonComponent.interactable = true;

            currentLocationInTheDungeon = true;
            mainCamera.SetActive(false);
            dungeonCamera.SetActive(true);

            for (int i = 0; i < panels.Count; i++)
            {
                panels[i].SetActive(false);
            }

            panelLabelObject.SetActive(true);
            panels[panelIndex].SetActive(true);

            if (LoadoutController.Instance != null)
                LoadoutController.Instance.Initialization();

            loadoutObject.SetActive(true);
            DungeonHubManager.dungeonHubManager.OpenPanel(0);
            return;
        }
        else
        {
            currentLocationInTheDungeon = false;
            mainCamera.SetActive(true);
            dungeonCamera.SetActive(false);

            loadoutObject.SetActive(false);
        }

        for (int i = 0; i < panels.Count; i++)
        {
            if (i == panelIndex)
            {
                if (panelIndex == 1 || panelIndex == 0)
                {
                    anvilSound.SetActive(true);
                    if (panelIndex != 1)
                        tapArea.SetActive(false);
                    else
                        tapArea.SetActive(true);

                }
                else
                {
                    anvilSound.SetActive(false);
                    tapArea.SetActive(false);
                }

                panels[i].SetActive(true);
            }
            else
            {
                panels[i].SetActive(false);
            }
        }

        panelLabelObject.SetActive(true);
    }

    [ContextMenu("Cool Down")]
    public void StartCoolDown()
    {
        coolDownCoroutine = StartCoroutine(CoolDown());
    }

    public IEnumerator CoolDown()
    {
        Debug.Log("Start Cool Down");

        currentTime = reloadTime;
        buttonToDungeon.interactable = false;
        timerRunning = false;
        buttonsToDungeonGroup[0].SetActive(false);
        buttonsToDungeonGroup[1].SetActive(true);

        coolDownText.text = currentTime.ToString("F2");

        while (!timerRunning)
        {
            yield return new WaitForSeconds(1f);
            currentTime -= 1;
            float seconds = currentTime % 60;
            string secondsText;
            if (seconds < 10)
                secondsText = 0 + seconds.ToString();
            else
                secondsText = seconds.ToString();

            coolDownText.text = (int)(currentTime / 60) + ":" + secondsText;

            if (currentTime <= 0)
                timerRunning = true;
        }

        coolDownCoroutine = null;
        buttonToDungeon.interactable = true;

        buttonsToDungeonGroup[0].SetActive(true);
        buttonsToDungeonGroup[1].SetActive(false);
    }

    public void EnableDungeonButton()
    {
        dungeonButtonComponent.interactable = true;
        dungeonButtonImageComponent.color = Color.white;
    }

    public void EnableBoostersButton()
    {
        boostersButtonComponent.interactable = true;
        boostersButtonImageComponent.color = Color.white;
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
        panelNameLabel.text = panelNames[panelIndex];
    }
}