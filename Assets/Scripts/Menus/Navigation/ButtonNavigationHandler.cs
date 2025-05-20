using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private UnityEngine.UI.Button _currentButton;
    [SerializeField] private Image backgroundImage;

    [SerializeField] private Color selectedColor;
    [SerializeField] private Color deselectedColor;
    void OnEnable()
    {
        backgroundImage.gameObject.SetActive(false);
        TextMeshProUGUI text = _currentButton.GetComponentInChildren<TextMeshProUGUI>();
        text.color = deselectedColor;
    }
    void Start()
    {
        _currentButton = GetComponent<UnityEngine.UI.Button>();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        TextMeshProUGUI text = _currentButton.GetComponentInChildren<TextMeshProUGUI>();
        text.color = deselectedColor;
        text.transform.position -= new Vector3(4f, 0f, 0);
        backgroundImage.gameObject.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {

        TextMeshProUGUI text = _currentButton.GetComponentInChildren<TextMeshProUGUI>();
        text.color = selectedColor;
        text.transform.position += new Vector3(4f, 0f, 0);
        backgroundImage.gameObject.SetActive(true);
    }
}
