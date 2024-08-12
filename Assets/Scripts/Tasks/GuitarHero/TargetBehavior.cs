using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    private float speed;
    [SerializeField] private GuitarHeroTask task;
    private bool _canBePressed;

    public float _isButtonPressed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _isButtonPressed = task._isButtonPressed;
        if(_canBePressed && _isButtonPressed>0)
        {
            Debug.Log("Acertou!");
            this.gameObject.SetActive(false);
        }
        else if(_isButtonPressed>0 && !_canBePressed)
        {
            Debug.Log("Errou!");
            
            this.gameObject.SetActive(false);
        }
    }
    
    void FixedUpdate()
    {
        if (this.gameObject.activeSelf)
        {
            body.velocity = Vector2.left * speed;

        }
    }
    void OnEnable()
    {
        speed = task.GetGameSpeed();
        _canBePressed = false;
    }
    void OnTriggerEnter2D()
    {
        _canBePressed = true;
    }
    void OnTriggerExit2D()
    {
        _canBePressed = false;
    }
}
