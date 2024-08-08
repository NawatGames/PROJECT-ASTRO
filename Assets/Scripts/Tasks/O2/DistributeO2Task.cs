using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DistributeO2Task : TaskScript
{
    [SerializeField] private RectTransform circle;
    [SerializeField] private RectTransform arrow;
    [SerializeField] private RectTransform specialZone;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float alignmentThreshold = 10f;
    [SerializeField] private float RequiredAlignments = 7;
    private bool _isRotating = false;
    private int _successfulAlignments = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void RunTask()
    {
        base.RunTask();
        StartRotation();
        PositionSpecialZone();
        StartCoroutine(RotateArrowCoroutine());
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
        arrow.RotateAround(circle.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private void CheckAlignment()
    {
        float angleDifference = Mathf.Abs(Vector3.SignedAngle(arrow.up, specialZone.position - circle.position, Vector3.forward));
        if (angleDifference <= alignmentThreshold)
        {
            _successfulAlignments++;
            rotationSpeed += 20;
            if (_successfulAlignments >= RequiredAlignments)
            {
                TaskSuccessful();
            }
            else
            {
                PositionSpecialZone();
            }
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
        StopRotation();
        StopAllCoroutines();
        _successfulAlignments = 0;
    }

    protected override void TaskMistakeStay()
    {
        base.TaskMistakeStay();
    }

    public override void EndTask()
    {
        base.EndTask();
        StopRotation();
        StopAllCoroutines();
    }

    private IEnumerator RotateArrowCoroutine()
    {
        while (_isRotating)
        {
            RotateArrowAroundCircle();
            yield return null;
        }
    }
}
