using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonEnemy : MonoBehaviour
{
    public float scale = 1.5f;
    public float distanceToCollider = 1.5f;
    public float enemyLVL;
    public GameObject chestPrefab;
    public EnemyHealthBar enemyHealthBar;
    public TextMeshProUGUI enemyLvlText;
    public AudioSource audioSource;
    public List<GameObject> currentEnemySkin = new List<GameObject>();
    public List<AudioClip> sounds = new List<AudioClip>();

    private int cuurentSkinIndex;
    private BoxCollider detectionCollider;
    private List<Animator> animators = new List<Animator>();

    public void PlayEnemyDeathSound()
    {
        audioSource.PlayOneShot(sounds[Random.Range(3, 5)]);
    }

    public void PlayEnemyDamageSound()
    {
        audioSource.PlayOneShot(sounds[Random.Range(0, 3)]);
    }

    public void Initialization()
    {
        AddAnimatorToAnimatorsList();
        //InitializeVariables();
        SetEnemySkin();
        //IncreaseBlueprintDropChance();
        SetScale();
        HandleDistanceCollider();
        SetEnemyLVL();
    }

    private void SetEnemyLVL()
    {
        if (enemyLVL == 0)
            enemyLvlText.text = (enemyLVL + 1).ToString() + " LVL";
        else
            enemyLvlText.text = enemyLVL.ToString() + " LVL";
    }

    //private void InitializeVariables()
    //{
    //    dropRate = PanelsHandler.Instance.dropChanceImprovements;
    //}

    private void SetScale()
    {
        transform.localScale = Vector3.one * scale;
    }

    private void AddAnimatorToAnimatorsList()
    {
        animators.Add(GetComponent<Animator>());
        animators.AddRange(GetComponentsInChildren<Animator>());
    }

    private void HandleDistanceCollider()
    {
        detectionCollider = GetComponent<BoxCollider>();
        detectionCollider.center = new Vector3(detectionCollider.center.x, detectionCollider.center.y, distanceToCollider);
    }

    private void SetEnemySkin()
    {
        if (SkinsManager.Instance.dungeonEnemySkinCount > 0)
        {
            int random = Random.Range(0, SkinsManager.Instance.dungeonEnemySkinCount + 1);

            Debug.Log("Enemy skin index: " + random);

            foreach (GameObject obj in currentEnemySkin)
            {
                obj.SetActive(false);
            }
            currentEnemySkin[random].SetActive(true);
            cuurentSkinIndex = random;
        }
        else
        {
            currentEnemySkin[0].SetActive(true);
        }
    }

    public void PlayDamageAnimation()
    {
        PlayEnemyDamageSound();

        for (int i = 0; i < animators.Count; i++)
            animators[i].Play("Take Damage", 0, 0);
    }

    public bool isEmpty = true;
    public int rewardStars = 0;
    public void InvokeDeathAnimation()
    {
        CheckReward();

        for (int i = 0; i < animators.Count; i++)
            animators[i].SetTrigger("Death");
        PlayEnemyDeathSound();
        Destroy(gameObject, 10f);
    }

    public void CheckReward()
    {
        if (isEmpty)
            return;

        if (CraftPanelItemsManager.Instance.currentWaitingPanel != null)//&& CraftPanelItemsManager.Instance.currentWaitingPanel.currentState == PanelItemState.WaitingForDrawing)
        {
            DungeonBuilder builder = DungeonBuilder.Instance;

            //builder.currentDropRate -= 1;
            //int value = builder.currentDropRate;

            //Debug.Log("Value - " + value + " | " + "Drop rate");

            //if (value <= 0)
            //{
            GameObject chestObject = Instantiate(chestPrefab, transform.position + new Vector3(0, 0.325f, 0), Quaternion.identity, transform.parent);
            DungeonWeaponBlueprint chest = chestObject.GetComponent<DungeonWeaponBlueprint>();

            //builder.currentStarValue += 1;

            switch (rewardStars)
            {
                case 1:
                    chest.Initialization(DungeonWeaponBlueprint.ChestFilling.money);
                    break;

                case 2:
                    chest.Initialization(DungeonWeaponBlueprint.ChestFilling.money);
                    break;

                case 3:
                    if (CraftPanelItemsManager.Instance.currentWaitingPanel.currentState == PanelItemState.WaitingForDrawing)
                    {
                        chest.Initialization(DungeonWeaponBlueprint.ChestFilling.drawing);
                        Debug.Log(CraftPanelItemsManager.Instance.currentWaitingPanel.currentState);
                    }
                    else
                        chest.Initialization(DungeonWeaponBlueprint.ChestFilling.money);

                    DungeonBuilder.Instance.currentStarValue = 0;
                    break;

                case 0:
                    break;
            }

            if (DungeonBuilder.Instance.bossCreated && rewardStars == 3)
            {
                DungeonBuilder.Instance.ClearDungeon();
            }

            //builder.currentDropRate = builder.maxDropRate;
            //}
        }
    }
}