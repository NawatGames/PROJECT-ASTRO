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
        [SerializeField] private float minDistance = 1f;
        //[SerializeField] private float maxDistance = 10f;
        [SerializeField] private bool playOnStart = false;

        private AudioSource _source;
        private List<GameObject> _players;
        
        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _source.clip = clip;
        }

        private void Start()
        {
            _players = GameObject.FindGameObjectsWithTag("Player").ToList();
            if(playOnStart) PlayLoop();
        }

        private float GetNearestPlayerDistance()
        {
            if (_players.Count <= 0)
            {
                return 0; // Força audio no max
            }
            return _players.Min(player => Vector2.Distance(player.transform.position, transform.position));
        }

        private float GetVolume(float distance)
        {
            if (distance <= minDistance) return 1f;
            return 1/(distance);
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