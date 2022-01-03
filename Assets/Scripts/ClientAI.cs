using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientAI : MonoBehaviour
{
    public bool active = false;
    public CanvasGroup reactionPanel;
    public CanvasGroup goodReaction;
    public CanvasGroup badReaction;

    public enum states
    {
        gotoSell,
        idle,
        goFromSell
    }
    public void Awake()
    {
        reactionPanel.alpha = 0f;
        goodReaction.alpha = 0f;
        badReaction.alpha = 0f;
    }

    public void FeedBack(bool react)
    {
        reactionPanel.alpha = 1;
        if (react == true)
        {
            goodReaction.alpha = 1f;
            StartCoroutine(FadeReaction(goodReaction));
        }
        else
        {
            badReaction.alpha = 1f;
            StartCoroutine(FadeReaction(badReaction));
        }
    }

    public IEnumerator FadeReaction(CanvasGroup group)
    {
        for (float f = 1.5f; f >= 0; f -= 0.05f)
        {
            reactionPanel.alpha = f;
            group.alpha = f;
            //Debug.Log(reactionPanel.alpha);
            //Debug.Log(group.alpha);

            yield return new WaitForSeconds(0.1f);
        }
        reactionPanel.alpha = 0;
        group.alpha = 0;
        active = false;
    }

    public void OnMouseDown()
    {
        // if(active == false)
        // {
        //     GameManager.gameManager.NewClient();
        //     GameManager.gameManager.setAi = this;
        // }
        // active = true;
    }
}
