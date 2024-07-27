using UnityEngine;

public class SignalLight : MonoBehaviour
{
    private SpriteRenderer _sprRnd;

    private void Awake()
    {
        _sprRnd = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _sprRnd.color = Color.black;
    }
}
