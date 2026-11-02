using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName="Scriptable Objects/TaskSO")]
public class TaskSO : ScriptableObject
{
    [Tooltip("Fallback: só é usado se nenhum TaskController tiver este SO no campo taskSO")]
    public string taskControllerCarrierName;
}
