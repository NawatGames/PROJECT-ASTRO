using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    public Transform pos;
    [SerializeField] private Rigidbody2D body;
    private float speed;
    [SerializeField] private GuitarHeroTask task;
    public bool _pressNow;

    public float _isButtonPressed;
    // Start is called before the first frame update
    void Start()
    {
        pos = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        _pressNow = false;
        gameObject.transform.position = pos.position;
    }
    void OnTriggerEnter2D()
    {
        _pressNow = true;
    }
    void OnTriggerExit2D()
    {
        _pressNow = false;
    }
}
