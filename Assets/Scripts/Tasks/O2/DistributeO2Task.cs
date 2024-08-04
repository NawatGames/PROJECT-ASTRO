using UnityEngine;
using UnityEngine.InputSystem;

public class DistributeO2Task : TaskScript
{
    [SerializeField] private RectTransform circle;
    [SerializeField] private RectTransform arrow;
    [SerializeField] private RectTransform specialZone;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float alignmentThreshold = 10f;

    private bool _isRotating = false;

    protected override void Awake()
    {
        base.Awake();
        PositionSpecialZone();
    }

    protected override void RunTask()
    {
        base.RunTask();
        StartRotation();
    }

    private void StartRotation()
    {
        _isRotating = true;
    }

    private void StopRotation()
    {
        _isRotating = false;
    }

    private void RotateArrowAroundCircle()
    {
        if (arrow != null && circle != null)
        {
            arrow.RotateAround(circle.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    private void CheckAlignment()
    {
        if (arrow != null && specialZone != null && circle != null)
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
    }

    private void PositionSpecialZone()
    {
        if (circle != null && specialZone != null)
        {
            float radius = circle.rect.width / 2;
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            Vector2 tangentialPosition = randomDirection * radius;

            specialZone.localPosition = tangentialPosition;

            float angle = Mathf.Atan2(randomDirection.y, randomDirection.x) * Mathf.Rad2Deg;
            specialZone.rotation = Quaternion.Euler(0, 0, angle + 90);
        }
    }

    protected override void OnUpPerformed(InputAction.CallbackContext value)
    {
        if (_isRotating)
        {
            CheckAlignment();
        }
    }

    protected override void TaskSuccessful()
    {
        base.TaskSuccessful();
        Debug.Log("Tarefa Completada!");
        StopRotation();
    }

    public override void EndTask()
    {
        base.EndTask();
        StopRotation();
    }

    private void Update()
    {
        if (_isRotating)
        {
            RotateArrowAroundCircle();
        }
    }
}
