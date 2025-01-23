using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{

    [SerializeField] private bool _occupied;
    private Transform _decontaminationTransform;
    
    void Start()
    {
        _decontaminationTransform = GetComponentInChildren<Transform>();
        _occupied = false;
    }

    public bool IsOccupied()
    {
        return _occupied;
    }

    public void SetOccupied(bool occupied)
    {
        _occupied = occupied;
    }

    public Vector2 GetDecontaminationPosition()
    {
        return _decontaminationTransform.position;
    }
}
