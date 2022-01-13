using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AchievementsManager : MonoBehaviour
{
    public static AchievementsManager Instance;

    public AchievementPanel achievementPopUp;
    public List<WeaponCountAchievement> weaponCountAchievements = new List<WeaponCountAchievement>();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < weaponCountAchievements.Count; i++)
        {
            weaponCountAchievements[i].currentCount = PlayerPrefs.GetInt("WeaponCountAchievementCount" + i);

            if (PlayerPrefs.GetString("WeaponCountAchievement" + i) == "achieved")
            {
                weaponCountAchievements[i].achieved = true;
            }
        }
    }

    public void CheckOnWeaponCountAchievement(DragObject weapon)
    {
        if (weaponCountAchievements.Exists(x => (x.achieved == false && x.weaponIndex == weapon.index)))
        {
            var achievementsList = weaponCountAchievements.FindAll(x => (x.achieved == false && x.weaponIndex == weapon.index));
            foreach (WeaponCountAchievement achievement in achievementsList)
            {
                achievement.currentCount++;
                PlayerPrefs.SetInt("WeaponCountAchievementCount" + achievement.index, achievement.currentCount);
                if (achievement.currentCount >= achievement.requiredCount)
                {
                    achievement.achieved = true;
                    PlayerPrefs.SetString("WeaponCountAchievement" + achievement.index, "achieved");
                    ShowAchievement(achievement.index);
                }
            }
        }
    }

    public void ShowAchievement(int index)
    {
        achievementPopUp.InitializeViewAsWeaponCount(weaponCountAchievements[index]);
        achievementPopUp.GetAchievementReward(weaponCountAchievements[index].reward);
        achievementPopUp.gameObject.SetActive(true);
    }
}

[System.Serializable]
public class Achievement
{
    public int index;
    public long reward;
    public string title;
    public string description;
    public bool achieved;
}

[System.Serializable]
public class WeaponCountAchievement : Achievement
{
    public int weaponIndex;
    public int requiredCount;
    public int currentCount;
}