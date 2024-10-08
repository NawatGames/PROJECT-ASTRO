using System;
using Player.StateMachine;
using UnityEngine;


public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorOverrideController astroAnimator;
    [SerializeField] private AnimatorOverrideController alienAnimator;

    private void Awake()
    {
        /* Descomentar após criar e associar os animators
        if (transform.parent.GetComponentInChildren<PlayerStateMachine>().isAstro)
        {
            animator.runtimeAnimatorController = astroAnimator;
        }
        else
        {
            animator.runtimeAnimatorController = alienAnimator;
        }
        */
    }
}
