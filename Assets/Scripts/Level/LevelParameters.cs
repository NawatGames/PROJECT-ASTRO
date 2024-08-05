using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Scriptable Objects/LevelParameters")]
public class LevelParameters : ScriptableObject
{
    #region Tasks
    
    public int maxActiveTasks;
    // timeForTaskToFail  (Ou colocar pra cada task)

    #endregion

    #region Alien

    public int alienInactiveAtStartSeconds;
    public float invasionDelaySeconds;
    public float invasionWarningSeconds;
    public int alienInsideSeconds;

    #endregion
}
