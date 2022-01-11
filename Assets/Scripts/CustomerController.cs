using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public static CustomerController Instance;
    private Animator animator;
    public GameObject chest;
    public GameObject anvil;
    private bool isAnvilPlaying;
    public ParticleSystem particles;
    public Animator swordAnvilAnimator;
    public GameObject hammer;

    int popusCounter;

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (CheckAnvil())
            return;
    }

    public void SpawnCurrencyPopUp()
    {
        MoneyHandler.Instance.SpawnCurrencyPopUp();
    }

    public void PlayAnvilParticles()
    {
        particles.Play();
        swordAnvilAnimator.Play("Jump Scale2", 0, 0);
    }

    private bool CheckAnvil()
    {
        if (anvil.activeSelf == true && !isAnvilPlaying)
        {
            transform.position = new Vector3(-1.93f, 0f, 4.9f);
            transform.rotation = Quaternion.Euler(0, -130, 0);
            hammer.SetActive(true);
            animator.Play("Anvil");
            isAnvilPlaying = true;
            return true;
        }

        if (anvil.activeSelf == true && isAnvilPlaying)
        {
            return true;
        }

        return false;
    }

    public void ChangeAnimation()
    {
        if (CheckAnvil())
            return;

        int val = Random.Range(0, 5);
        if (val == 0)
        {
            transform.position = new Vector3(0f, 0f, 0.23f);
            transform.rotation = Quaternion.Euler(0, 180, 0);

            animator.Play("Idle");
        }
        else if (val == 1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);

            transform.position = new Vector3(1.56f, -0.2f, 9.22f);
            animator.Play("Sit");
        }
        else if (val == 2 && chest.activeSelf == true)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.position = new Vector3(3.35f, 0.053f, 8.52f);
            animator.Play("Sit Chest");
        }
        else if (val == 3)
        {
            transform.position = new Vector3(0f, 0f, 4.5f);
            transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);
            animator.Play("Warmup");
        }
        else if (val == 4)
        {
            transform.position = new Vector3(-5.55f, -0.3f, 6.55f);
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
            animator.Play("Inspect");
        }
    }
}