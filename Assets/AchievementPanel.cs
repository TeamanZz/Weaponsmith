using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementPanel : MonoBehaviour
{
    private Animator animator;
    public ParticleSystem particles;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {

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
        SFX.Instance.PlaySell();
        // particles.Play();
    }
}