using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{

    [SerializeField] private bool _occupied;
    void Start()
    {
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
}
