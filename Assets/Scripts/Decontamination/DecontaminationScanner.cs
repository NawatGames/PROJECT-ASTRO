using UnityEngine;

public class DecontaminationScanner : MonoBehaviour
{
    [SerializeField] private DecontaminationTask decontaminationTask;
    
    public void ScanAnimationEndHandler() // Chamada por AnimationEvent no fim da animação de scan
    {
        StartCoroutine(decontaminationTask.CompleteDecontamination());
    }
}
