using System;
using Player.StateMachine;
using UnityEngine;


public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    public void SetMovementAnimParameters(Vector2 moveVector)
    {
        float lastHorizontal = animator.GetFloat("Horizontal");
        float lastVertical = animator.GetFloat("Vertical");
        
        
        // Se ambos last forem pra 0, o idle é forçado pra direção padrão (cima)
        if (lastHorizontal != 0f || lastVertical != 0f)
        {
            animator.SetFloat("LastHorizontal", lastHorizontal);
            animator.SetFloat("LastVertical", lastVertical);
        }
        
        animator.SetFloat("Horizontal", moveVector.x);
        animator.SetFloat("Vertical", moveVector.y);
    }
}