using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName="Scriptable Objects/TaskSO")]
public class TaskSO : ScriptableObject
{
    [Tooltip("Deve ser exatamente igual ao nome do objeto da task em cena (que possui TaskController)")]
    public string taskControllerCarrierName;
}
