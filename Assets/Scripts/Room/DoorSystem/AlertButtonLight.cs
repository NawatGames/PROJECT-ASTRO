using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AlertButtonLight : MonoBehaviour
{
    // Start is called before the first frame update
    Light2D light;
    void Start()
    {
        light = GetComponent<Light2D>();
        light.enabled = false;
    }

    public void Quarantine()
    {
        light.color = Color.red;
        light.enabled = true;
    }

    public void NotQuarantine()
    {
        light.enabled = false;
    }
   
   
}
