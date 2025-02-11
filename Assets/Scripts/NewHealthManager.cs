using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHealthManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int health;
    [SerializeField] private int preGameOverDelay;
    [SerializeField] GameEvent onZeroHealthEvent;

    void Awake()
    {
        health = 3;
    }

    [ContextMenu("Forçar perda de vida")]
    public void DecreaseHealth()
    {
        if(health > 1)
        {
            health--;
            animator.SetTrigger("decreaseHealth");
        }

        else
        {
            health = 3;
            animator.SetTrigger("decreaseHealth");
            StartCoroutine(StartGameOver());
        }
    }

    IEnumerator StartGameOver()
    {
        yield return new WaitForSeconds(preGameOverDelay);
        onZeroHealthEvent.Raise();
    }
}
