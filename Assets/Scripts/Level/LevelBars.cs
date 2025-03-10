using System.Collections;
using UnityEngine;
using TMPro;

public class LevelBars : MonoBehaviour
{
    [Header("Sprites do Timer")]
    [SerializeField] private Sprite tempo0;
    [SerializeField] private Sprite tempo12;
    [SerializeField] private Sprite tempo34;
    [SerializeField] private Sprite tempo50;
    [SerializeField] private Sprite tempo68;
    [SerializeField] private Sprite tempo87;
    [SerializeField] private Sprite tempo95;
    [SerializeField] private Sprite tempo100;

    [Header("Percentuais de Mudança")]
    [SerializeField] private int percent0 = 0;
    [SerializeField] private int percent12 = 12;
    [SerializeField] private int percent34 = 34;
    [SerializeField] private int percent50 = 50;
    [SerializeField] private int percent68 = 68;
    [SerializeField] private int percent87 = 87;
    [SerializeField] private int percent95 = 95;
    [SerializeField] private int percent100 = 100;

    [Header("Configuração do Timer")]
    [SerializeField] private int countdownTillVictory = 120;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameEvent levelCompleted;

    private void Start()
    {
        if (spriteRenderer == null) return;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        SetSprite(tempo0);

        yield return new WaitForSeconds(countdownTillVictory * ((percent12 - percent0) / 100f));
        SetSprite(tempo12);
        Debug.Log(percent12);

        yield return new WaitForSeconds(countdownTillVictory * ((percent34 - percent12) / 100f));
        SetSprite(tempo34);
        Debug.Log(percent34);

        yield return new WaitForSeconds(countdownTillVictory * ((percent50 - percent34) / 100f));
        SetSprite(tempo50);
        Debug.Log(percent50);

        yield return new WaitForSeconds(countdownTillVictory * ((percent68 - percent50) / 100f));
        SetSprite(tempo68);
        Debug.Log(percent68);

        yield return new WaitForSeconds(countdownTillVictory * ((percent87 - percent68) / 100f));
        SetSprite(tempo87);
        Debug.Log(percent87);

        yield return new WaitForSeconds(countdownTillVictory * ((percent95 - percent87) / 100f));
        SetSprite(tempo95);
        Debug.Log(percent95);

        yield return new WaitForSeconds(countdownTillVictory * ((percent100 - percent95) / 100f));
        SetSprite(tempo100);
        Debug.Log(percent100);

        Debug.Log("Victory!");
        levelCompleted.Raise();
    }

    private void SetSprite(Sprite sprite)
    {
        if (sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}
