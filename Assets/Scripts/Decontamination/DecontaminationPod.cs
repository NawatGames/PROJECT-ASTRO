using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DecontaminationPod : MonoBehaviour
{
    [SerializeField] private Transform decontaminationTransform;
    [SerializeField] private GameEvent decontaminationOccupancyChanged;
    private bool _occupied;

    
    [Conditional("UNITY_EDITOR")]
    private void WarnIfPositioningIsWrong()
    {
        if (!GetComponent<BoxCollider2D>().bounds.Contains(decontaminationTransform.position))
        {
            Debug.LogWarning($"O Player Positioning deve estar dentro do collider do {name}! (Se estiver perto o suficiente, ignorar)");
        } 
    }
    
    void Start()
    {
        // Apenas para debug:
        WarnIfPositioningIsWrong();
    }

    public bool IsOccupied()
    {
        return _occupied;
    }

    public void SetOccupied(bool occupied)
    {
        _occupied = occupied;
        decontaminationOccupancyChanged.Raise();
    }

    public Vector2 GetDecontaminationPosition()
    {
        return decontaminationTransform.position;
    }
}
