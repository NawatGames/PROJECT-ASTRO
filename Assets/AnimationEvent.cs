using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public UnityEvent[] OnAnimationEvent;

    public void AnimationEventTrigger(int eventIndex)
    {
        if (eventIndex >= 0 && eventIndex < OnAnimationEvent.Length)
        {
            OnAnimationEvent[eventIndex]?.Invoke();
        }
        else
        {
            Debug.LogWarning("invalid animation event index");
        }
    }
}
