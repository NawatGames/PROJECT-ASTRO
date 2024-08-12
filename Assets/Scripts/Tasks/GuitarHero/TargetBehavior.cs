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
    public int symbol;

    public bool _passedBeyondTrigger = false;

    public float _isButtonPressed;
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        DebugColor();
    }

    void FixedUpdate()
    {
        if (this.gameObject.activeSelf)
        {
            body.velocity = Vector2.left * speed;

        }
    }
    void DebugColor()
    {
        switch (symbol)
        {
            case 0:
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                break;

            case 1:
                gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case 3:
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            default:
                break;
        }

    }
    void OnEnable()
    {
        speed = task.GetGameSpeed();
        _pressNow = false;
        gameObject.transform.position = pos.position;
        symbol = Random.Range(0, 3);

    }
    void OnTriggerEnter2D()
    {
        _pressNow = true;
    }
    void OnTriggerExit2D()
    {
        _pressNow = false;
        task.InsertTargetInBuffer();
    }
}
