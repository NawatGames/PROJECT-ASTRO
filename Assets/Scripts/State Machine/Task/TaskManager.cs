using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<TaskController> taskList;
    
    
    
    void Start()
    {
        try
        {
            taskList[0].NeedsToBeDone = true;
        }
        catch
        {
            Debug.LogError("Preencher lista do obj TaskManager");
        }
    }
}
