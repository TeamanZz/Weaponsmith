using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSellBorder : MonoBehaviour
{
    public static ItemSellBorder Instance;
    public GameObject destroyParticles;
    public GameObject rewardTextPrefab;
    public Vector3 rewardTextSpawnPoint = new Vector3(0, 0, 0.3f);
    public Canvas tableCanvas;
    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        DragObject dragObject;
        if (other.TryGetComponent<DragObject>(out dragObject))
        {
            if (dragObject.itemType != ItemType.Whole)
                return;

            SellItem(dragObject);
            MakeBorderWhite();
        }
    }

    public void SellItem(DragObject dragObject)
    {
        var newParticles = Instantiate(destroyParticles, dragObject.transform.position + new Vector3(0, 0.1f, 0), Quaternion.Euler(-90, 0, 0));
        Destroy(dragObject.gameObject);
        int reward = MoneyHandler.Instance.GetRewardForCraft(dragObject.index);
        SpawnRewardText(reward);
        SFX.Instance.PlaySell();
        FTUEManager.Instance.ChangeFTUEState(7);
    }

    private void SpawnRewardText(int reward)
    {
        GameObject newRewardText = Instantiate(rewardTextPrefab, tableCanvas.transform);
        newRewardText.transform.localRotation = Quaternion.Euler(0, 51, -90);
        newRewardText.GetComponent<TextMeshProUGUI>().text = "+ " + FormatNumsHelper.FormatNum((double)reward) + " $";
    }

    public void MakeBorderGreen()
    {
        GetComponent<Image>().color = Color.green;
    }

    public void MakeBorderWhite()
    {
        GetComponent<Image>().color = Color.white;
    }
}