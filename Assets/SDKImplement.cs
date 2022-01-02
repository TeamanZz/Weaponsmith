using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class SDKImplement : MonoBehaviour
{
    private void Start()
    {
        GameAnalytics.Initialize();
    }
}
