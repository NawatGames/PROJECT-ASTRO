using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

    [SerializeField] private KeyCode inputKey = KeyCode.Space;
    [SerializeField] private Animator animator;
    
    private void Update()
    {
        if (Input.GetKeyDown(inputKey))
        {
            bool currentState = animator.GetBool("isBroken");
            animator.SetBool("isBroken", !currentState);
        }
    }
}
