using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private RectTransform _circle;
    [SerializeField] private RectTransform _arrow;
    [SerializeField] private float _rotationSpeed = 100f;

    void Update()
    {
        RotateArrowAroundCircle();
    }

    private void RotateArrowAroundCircle()
    {
        _arrow.RotateAround(_circle.position, Vector3.forward, _rotationSpeed * Time.deltaTime);
    }
}