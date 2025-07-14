using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool debugMode = false;
    [Header("SYSTEM SETTINGS")]

    [Header("Selection Colors")]
    [SerializeField] private Color backgroundSelectedColor;
    [SerializeField] private Color textSelectedColor;
    [Header("Unselection Colors")]
    [SerializeField] private Color backgroundDeselectedColor;
    [SerializeField] private Color textDeselectedColor;
    private UnityEngine.UI.Button _currentButton;
    private Image _backgroundImage;
    private TextMeshProUGUI _buttonText;

    void Awake()
    {
        _currentButton = GetComponent<UnityEngine.UI.Button>();
        _backgroundImage = GetComponentInChildren<Image>();
        _buttonText = GetComponentInChildren<TextMeshProUGUI>();

        _backgroundImage.color = backgroundDeselectedColor;
        _buttonText.color = textDeselectedColor;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_currentButton.interactable)
        {
            DebugInfo("Pointer entered button area.");
            _backgroundImage.color = backgroundSelectedColor;
            _buttonText.color = textSelectedColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_currentButton.interactable)
        {
            DebugInfo("Pointer exited button area.");
            _backgroundImage.color = backgroundDeselectedColor;
            _buttonText.color = textDeselectedColor;
        }
    }
    private void DebugInfo(string message)
    {
        if (!debugMode) return;

        Debug.Log($"<color=blue>ButtonHandler:</color> {message} - Button: {_currentButton.name}");
    }
}
