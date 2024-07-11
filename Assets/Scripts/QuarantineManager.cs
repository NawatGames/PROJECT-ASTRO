using Unity.VisualScripting;
using UnityEngine;

public class QuarantineManager : MonoBehaviour
{
    [SerializeField] private Collider2D roomArea;

    void OnEnable()
    {
        roomArea.enabled = false;
    }
    
    public void SetQuarantine()
    {
        roomArea.enabled = roomArea.enabled!;
        print("Quarentena mudou");
    }
}
