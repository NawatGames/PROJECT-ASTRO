using System;
using UnityEngine;

[CreateAssetMenu(menuName="Scriptable Objects/LevelParameters")]
public class LevelParameters : ScriptableObject
{
    #region Tasks
    
    [Header("Tasks")]
    public int maxActiveTasks = 6;
    public int startingTasks = 3;
    public float taskTimeWindow = 60;
    public float taskWarningTimeRatio = 1/3f;

    #endregion

    #region Alien

    [Header("Alien")]
    public float alienInactiveAtStartSeconds = 8;
    public float invasionDelaySeconds = 20;
    public float invasionWarningSeconds = 8;
    public float alienInsideSeconds = 8;

    #endregion
}
