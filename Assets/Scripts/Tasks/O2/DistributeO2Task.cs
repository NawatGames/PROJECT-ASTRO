using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DistributeO2Task : TaskScript
{
    [SerializeField] private Transform circle;
    [SerializeField] private Transform arrow;
    [SerializeField] private Transform specialZone;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float alignmentThreshold;
    [SerializeField] private float RequiredAlignments = 7;
    [SerializeField] private int maxUnsuccessfulAlignments = 3;
    private int _unsuccessfulAlignments = 0;
    private bool _isRotating = false;
    private int _successfulAlignments = 0;

    protected override void Awake()
    {
        base.Awake();
        taskName = "O2 task";
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

    [ContextMenu("CheckAlign")]
    private void CheckAlignment()
    {
        float angleDifference = Mathf.Abs(arrow.eulerAngles.z - specialZone.eulerAngles.z);

        if (angleDifference <= alignmentThreshold)
        {

            if (isAstro == isAstroSpecialist)
            {
                _successfulAlignments += 2;
            }
            else
            {
                _successfulAlignments++;
            }

            if (_successfulAlignments >= RequiredAlignments)
            {
                TaskSuccessful();
                rotationSpeed += 20;
            }
            else
            {
                PositionSpecialZone();
            }
        }
        else if(angleDifference > alignmentThreshold)
        {
            TaskMistakeStay();
            _unsuccessfulAlignments++;

            if (_unsuccessfulAlignments >= maxUnsuccessfulAlignments)
            {
                TaskMistakeLeave();
            }
        }
    }

    private void PositionSpecialZone()
    {
        // Gera um ângulo aleatório entre 0 e 360 graus
        float randomAngle = Random.Range(0f, 360f);

        // Aplica a rotação diretamente na seta
        specialZone.rotation = Quaternion.Euler(0, 0, randomAngle);
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
        _unsuccessfulAlignments = 0; // Reinicia também os erros
    }

    protected override void TaskMistakeStay()
    {
        base.TaskMistakeStay();
    }

    public override void EndTask()
    {
        base.EndTask();
        _unsuccessfulAlignments = 0;
        _successfulAlignments = 0;
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
