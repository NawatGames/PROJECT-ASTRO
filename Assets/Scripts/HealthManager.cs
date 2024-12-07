using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private GameEvent onZeroHealth;
    [SerializeField] private TextMeshProUGUI healthText;

    private void Start()
    {
        healthText.text = "VIDA: " + health;
    }

    // TESTE TIRAR DEPOIS
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DecreaseHealth();
        }
    }

    public void DecreaseHealth()
    {
        health -= 1;
        healthText.text = "VIDA: " + health;
        if (health == 0)
        {
            onZeroHealth.Raise();
        }
    }
}
