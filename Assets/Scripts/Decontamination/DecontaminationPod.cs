using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecontaminationPod : MonoBehaviour
{

    private bool _occupied;
    private Transform _decontaminationTransform;

    [SerializeField] private GameEvent DecontaminationOccupancyChanged;
    void Start()
    {
        _decontaminationTransform = transform.GetChild(0);
        
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
        DecontaminationOccupancyChanged.Raise();
    }

    public Vector2 GetDecontaminationPosition()
    {
        return _decontaminationTransform.position;
    }
}
