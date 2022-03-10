using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnWindowAppear : MonoBehaviour
{
    public bool activateOnEnable;
    public bool activateOnStart;

    private void OnEnable()
    {
        if (activateOnEnable)
        {
            SFX.Instance.PlayWindowAppear();
        }
    }

    private void Start()
    {
        if (activateOnStart)
        {
            SFX.Instance.PlayWindowAppear();
        }
    }
}
