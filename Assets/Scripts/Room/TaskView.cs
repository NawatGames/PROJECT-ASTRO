using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskView : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on TaskView.");
        }
    }

    public void SetIsBroken(bool isBroken)
    {
        if (animator != null)
        {
            animator.SetBool("isBroken", isBroken);
        }
    }
}
