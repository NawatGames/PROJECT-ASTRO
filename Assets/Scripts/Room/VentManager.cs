using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class VentManager : MonoBehaviour
{
    private SpriteRenderer ventSprite;
    [SerializeField] private Transform room;

    private void Awake()
    {
        ventSprite = GetComponent<SpriteRenderer>();
        room = transform.parent;
    }

    public void StartInvasionWarning(Component sender, object data)
    {
        if (sender == room)
            ventSprite.color = Color.magenta;
    }

    public void EndInvasionWarning(Component sender, object data)
    {
        if (sender == room)
            ventSprite.color = Color.green;
    }
}
