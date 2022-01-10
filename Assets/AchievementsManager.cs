using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AchievementsManager : MonoBehaviour
{
    public static AchievementsManager Instance;

    public List<Achievement> generalAchievementsList = new List<Achievement>();

    private void Awake()
    {
        Instance = this;
    }

    public void CheckOnAchievement(DragObject weapon)
    {
        // if (weapon.index )
    }
}

[System.Serializable]
public class Achievement
{
    public string title;
    public string description;
}

[System.Serializable]
public class AchievementCondition
{
    AchievementConditionType type;
}

public enum AchievementConditionType
{
    CraftItem,
    Money,
}