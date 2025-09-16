using Audio_System;
using UnityEngine;

public class VentManager : MonoBehaviour
{
    private Animator _animator;
    private string _actualTrigger = "AlienBase";
    [SerializeField] private Transform room;
    [SerializeField] private AudioPlayer alienShakingVent;
    [SerializeField] private AudioPlayer alienQuarantinedCrawl;
    [SerializeField] private AudioPlayer alienOpenedVent;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        room = transform.parent;
    }

    public void StartInvasionWarning(Component sender, object data)
    {
        if (sender == room)
        {
            //Debug.Log("InvasionStart");
            alienShakingVent.PlayLoop();
            _actualTrigger = "AlienInvading";
            _animator.SetTrigger(_actualTrigger);
        }
    }

    public void EndInvasionWarning(Component sender, object data)
    {
        if (sender == room)
        {
            //Debug.Log("InvasionEnd");
            alienQuarantinedCrawl.StopAudio();
            _actualTrigger = "AlienBase";
            _animator.SetTrigger(_actualTrigger);
        }
    }
    public void AlienAttacked(Component sender, object data)
    {
        if (sender == room)
        {
            //Debug.Log("AlienAttack");
            alienShakingVent.StopAudio();
            alienQuarantinedCrawl.StopAudio();
            alienOpenedVent.PlayAudio();
            _actualTrigger = "AlienInvaded";
            _animator.SetTrigger(_actualTrigger);
        }
    }
    public void InvasionQuarantined(Component sender, object data)
    {
        if (sender == room)
        {
            //Debug.Log("AlienQuarantined");
            alienShakingVent.StopAudio();
            alienQuarantinedCrawl.PlayLoop();
            _actualTrigger = "AlienQuarantined";
            _animator.SetTrigger(_actualTrigger);
        }
    }
}
