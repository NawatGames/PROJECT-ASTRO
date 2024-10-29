using System;
using Player.StateMachine;
using UnityEngine;


public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    public void SetMovimentAnimation(Vector2 moveVector)
    {
        animator.SetFloat("LastHorizontal", animator.GetFloat("Horizontal"));
        animator.SetFloat("LastVertical", animator.GetFloat("Vertical"));
        animator.SetFloat("Horizontal", moveVector.x);
        animator.SetFloat("Vertical", moveVector.y);
    }
}