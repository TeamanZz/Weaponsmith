using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreInitialization : MonoBehaviour
{
    public SkillController skillController;

    public void Awake()
    {
        skillController.Awake();    
    }
}
