using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorArrow : MonoBehaviour
{
    private Animator arrowAnimator;
    public int animationIndex;

    private void Awake() 
    {
        arrowAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (animationIndex == 0)
            arrowAnimator.Play("ArrowCursorAnimation");
        if (animationIndex == 1)
            arrowAnimator.Play("ArrowCursorAnimationHorizontal");
        if (animationIndex == 2)
            arrowAnimator.Play("ArrowCursorAnimationVertical2");
    }
}