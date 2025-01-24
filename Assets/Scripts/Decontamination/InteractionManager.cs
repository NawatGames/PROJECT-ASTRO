using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{

    [SerializeField] private bool _occupied;
    private Transform _decontaminationTransform;
    
    void Start()
    {
        _decontaminationTransform = transform.GetChild(0);
        _occupied = false;
        
        // Apenas para debug:
        if (!GetComponent<BoxCollider2D>().bounds.Contains(_decontaminationTransform.position))
        {
            Debug.LogWarning($"O Player Positioning deve estar dentro do collider do {name}! (Se estiver perto o suficiente, ignorar)");
        }
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
