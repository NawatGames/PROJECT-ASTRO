using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private KeyCode moveUpKey = KeyCode.W;
    [SerializeField] private KeyCode moveDownKey = KeyCode.S;
    [SerializeField] private KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode moveRightKey = KeyCode.D;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float driftFactor = 0.95f;

    private Vector2 _velocity;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 moveInput = Vector2.zero;

        if (Input.GetKey(moveUpKey))
        {
            moveInput.y += 1;
        }
        if (Input.GetKey(moveDownKey))
        {
            moveInput.y -= 1;
        }
        if (Input.GetKey(moveLeftKey))
        {
            moveInput.x -= 1;
        }
        if (Input.GetKey(moveRightKey))
        {
            moveInput.x += 1;
        }

        moveInput.Normalize();
        _velocity = moveInput * moveSpeed;
    }

    private void FixedUpdate()
    {
        _rb.velocity = _rb.velocity * driftFactor + _velocity * (1 - driftFactor);
    }
}