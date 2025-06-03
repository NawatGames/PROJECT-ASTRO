using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AlertButtonLight : MonoBehaviour
{
    // Start is called before the first frame update
    Light2D alertButtonLight;
    void Start()
    {
        alertButtonLight = GetComponent<Light2D>();
        alertButtonLight.enabled = false;
    }

    public void Quarantine()
    {
        alertButtonLight.color = Color.red;
        alertButtonLight.enabled = true;
    }

    public void NotQuarantine()
    {
        alertButtonLight.enabled = false;
    }
    
}
