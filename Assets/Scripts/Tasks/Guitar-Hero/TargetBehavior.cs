using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private GuitarHeroTask task;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    public Transform pos;
    private float speed;
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

        int symbolVal = Random.Range(0, 4);
        symbol = (SymbolEnum) symbolVal;
        spriteRenderer.sprite = sprites[symbolVal];
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
