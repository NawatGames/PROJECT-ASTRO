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

    private bool isRotating = false;

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
        isRotating = true;
    }

    private void StopRotation()
    {
        isRotating = false;
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

    protected override void OnUpPerformed(InputAction.CallbackContext value)
    {
        if (isRotating)
        {
            CheckAlignment();
        }
    }

    private void Update()
    {
        if (isRotating)
        {
            RotateArrowAroundCircle();
        }
    }

    protected override void TaskSuccessful()
    {
        base.TaskSuccessful();
        Debug.Log("Sucesso na Task");
        PositionSpecialZone();
        rotationSpeed +=20f;
    }

    protected override void TaskMistakeStay()
    {
        base.TaskMistakeStay();
        Debug.Log("Errou");

    }
}
