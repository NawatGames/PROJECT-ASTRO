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
    public SymbolEnum symbol;
    public bool _passedBeyondTrigger = false;

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
        transform.position = pos.position;
        symbol = (SymbolEnum) Random.Range(0, 4);
    }

    void OnDisable()
    {
        transform.position = pos.position;
    }

    void OnTriggerEnter2D()
    {
        _pressNow = true;
    }

    void OnTriggerExit2D()
    {
        _pressNow = false;
        if (gameObject.activeSelf)
        {
            task.InsertTargetInBuffer();
            // método para aumentar os erros
            task.IncrementMistake(); 
        }
    }
}
