using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private RectTransform circle;
    [SerializeField] private RectTransform arrow;
    [SerializeField] private RectTransform specialZone;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float alignmentThreshold = 10f;

    private bool _isRotating = false;

    void Start()
    {
        PositionSpecialZone();
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartRotation();
        }

        if (_isRotating)
        {
            RotateArrowAroundCircle();

            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                CheckAlignment();
            }
        }
    }

    public void StartRotation()
    {
        _isRotating = true;
    }

    public void StopRotation()
    {
        _isRotating = false;
    }

    private void RotateArrowAroundCircle()
    {
        arrow.RotateAround(circle.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private void CheckAlignment()
    {
        float angleDifference = Mathf.Abs(Vector3.SignedAngle(arrow.up, specialZone.position - circle.position, Vector3.forward));
        if (angleDifference <= alignmentThreshold)
        {
            TaskSuccessful();
        }
        else
        {
            TaskMistakeStay();
        }
    }

    private void PositionSpecialZone()
    {
        float radius = circle.rect.width / 2;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 tangentialPosition = randomDirection * radius;

        specialZone.localPosition = tangentialPosition;

        float angle = Mathf.Atan2(randomDirection.y, randomDirection.x) * Mathf.Rad2Deg;
        specialZone.rotation = Quaternion.Euler(0, 0, angle + 90);
    }

    private void TaskSuccessful()
    {
        PositionSpecialZone();
        rotationSpeed += 30f;
    }

    private void TaskMistakeStay()
    {
        Debug.LogError("Errou");
    }
}
