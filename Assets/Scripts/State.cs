using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool IsComplete { get; protected set; }
    protected float StartTime;
    public float CurrentTime => Time.time - StartTime;
    
    public virtual void Enter() {}
    public virtual void Do() {}
    public virtual void FixedDo() {}
    public virtual void Exit() {}
}