using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> stepsAudioClips;
    [SerializeField] private float stepAudioInterval = .75f;
    private int _index = 0;
    
    public IEnumerator PlayStepsAudioRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(stepAudioInterval);
            audioSource.PlayOneShot(stepsAudioClips[_index]);
            //_index = (_index+1)%stepsAudioClips.Count;
            _index = Random.Range(0, stepsAudioClips.Count);
        }
    }
}
