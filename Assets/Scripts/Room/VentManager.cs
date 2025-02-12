using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class VentManager : MonoBehaviour
{
    private SpriteRenderer ventSprite;
    private Animator _animator;
    private string _actualTrigger = "AlienBase";
    [SerializeField] private Transform room;

    private void Awake()
    {
        ventSprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        room = transform.parent;
    }

    public void StartInvasionWarning(Component sender, object data)
    {
        if (sender == room)
        {
            Debug.Log("InvasionStart");
            _actualTrigger = "AlienInvading";
            ventSprite.color = Color.magenta;
            _animator.SetTrigger(_actualTrigger);
        }
    }

    public void EndInvasionWarning(Component sender, object data)
    {
        if (sender == room)
        {
            Debug.Log("InvasionEnd");
            _actualTrigger = "AlienBase";
            ventSprite.color = Color.white;
            _animator.SetTrigger(_actualTrigger);
        }
    }
    public void AlienAttacked(Component sender, object data)
    {
        if (sender == room)
        {
            Debug.Log("AlienAttack");
            _actualTrigger = "AlienInvaded";
            ventSprite.color = Color.red;
            _animator.SetTrigger(_actualTrigger);
        }
    }
    public void InvasionQuarantined(Component sender, object data)
    {
        if (sender == room)
        {
            Debug.Log("AlienQuarantined");
            _actualTrigger = "AlienQuarantined";
            ventSprite.color = Color.blue;
            _animator.SetTrigger(_actualTrigger);
        }
    }
}
