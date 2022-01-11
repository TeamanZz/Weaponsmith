using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementPanel : MonoBehaviour
{
    private Animator animator;
    public ParticleSystem particles;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    public void InitializeViewAsWeaponCount(WeaponCountAchievement achievement)
    {
        titleText.text = "Achievement unlocked: " + achievement.title;
        descriptionText.text = achievement.description;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(IEPlayFadeOutAnimation());
        StartCoroutine(IEPlayParticles());
    }

    private IEnumerator IEPlayFadeOutAnimation()
    {
        yield return new WaitForSeconds(5);
        animator.Play("Achievement Fade Out");
        yield return new WaitForSeconds(1);
        particles.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private IEnumerator IEPlayParticles()
    {
        yield return new WaitForSeconds(0.8f);
        particles.gameObject.SetActive(true);
        SFX.Instance.PlayBuy();
    }
}