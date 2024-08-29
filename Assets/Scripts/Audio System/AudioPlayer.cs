using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Audio_System
{
    
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private AnimationCurve volumeOverDistance;
        [SerializeField] private bool playOnStart = false;

        private AudioSource _source;
        private List<PlayerController> _players;
        
        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _source.clip = clip;
        }

        private void Start()
        {
            _players = FindObjectsOfType<PlayerController>().ToList();
            if(playOnStart) PlayLoop();
        }

        private float GetNearestPlayerDistance()
        {
            return _players.Min(player => Vector2.Distance(player.transform.position, transform.position));
        }

        private float GetVolume(float distance)
        {
            return volumeOverDistance.Evaluate(distance);
        }
        
        public void PlayAudio()
        {
            _source.loop = false;
            _source.PlayOneShot(clip);
        }

        public void PlayLoop()
        {
            _source.loop = true;
            _source.Play();
        }
        
        public void StopAudio()
        {
            _source.Stop();
        }

        private void FixedUpdate()
        {
            if (!_source.isPlaying) return;
            float distance = GetNearestPlayerDistance();
            _source.volume = GetVolume(distance);
        }
    }
}