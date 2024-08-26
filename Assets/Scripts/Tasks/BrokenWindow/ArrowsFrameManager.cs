using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsFrameManager : MonoBehaviour
{
    private List<GameObject> _arrowsSequence;
    private int _currentArrowIndex = 0;

    private void Awake()
    {
        _arrowsSequence = new List<GameObject>();
    }

    public void AddArrow(GameObject arrow, int index, int sequenceSize)
    {
        GameObject arrowObj;
        _arrowsSequence.Add(arrowObj = Instantiate(arrow, transform));
        float x = (index) - (sequenceSize - 1) / 2f;
        arrowObj.transform.localPosition = new Vector3(x, 0, 0);
    }

    public void ResetArrowFrame()
    {
        _currentArrowIndex = 0;
        foreach (GameObject arrow in _arrowsSequence)
        {
            Destroy(arrow);
        }
        _arrowsSequence.Clear();
    }

    public void MarkArrow()
    {
        _arrowsSequence[_currentArrowIndex].GetComponent<SpriteRenderer>().color = Color.green;
        _currentArrowIndex++;
    }
}