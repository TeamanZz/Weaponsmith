using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnemy : MonoBehaviour
{
    private DungeonCharacter dungeonCharacter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<DungeonCharacter>(out dungeonCharacter))
        {
            StartCoroutine(IEDeath());
        }
    }

    public void InvokeDeathAnimation()
    {
        animator.SetTrigger("Death");
    }

    private IEnumerator IEDeath()
    {
        yield return new WaitForSeconds(0.7f);

    }
}