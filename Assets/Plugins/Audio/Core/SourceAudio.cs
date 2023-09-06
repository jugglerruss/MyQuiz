using System.Collections;
using UnityEngine;

namespace Plugins.Audio.Core
{
    public class SourceAudio : MonoBehaviour
    {
        [HideInInspector] public bool Loop;
    
        public float Volume
        {
            get => _audioSource.volume;
            set => _audioSource.volume = value;
        }
        
        public bool Mute
        {
            get => _audioSource.mute;
            set => _audioSource.mute = value;
        }

        public float Pitch
        {
            get => _audioSource.pitch;
            set => _audioSource.pitch = value;
        }
        
        private AudioSource _audioSource
        {
            get
            {
                if (_audioSourceCech == null)
                {
                    _audioSourceCech = GetComponent<AudioSource>();
                    _audioSource.clip = null;
    
                    Loop = _audioSource.loop;
                    _audioSource.loop = false;
                }
                
                return _audioSourceCech;
            }
        }
        
        private AudioSource _audioSourceCech;
        private Coroutine _playRoutine;
    
        public void Play(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("key is empty, Source Audio PlaySound: " + gameObject.name);
                return;
            }

            if (_playRoutine != null)
            {
                StopCoroutine(_playRoutine);
            }
            
            _playRoutine = StartCoroutine(PlayRoutine(key));
        }

        private IEnumerator PlayRoutine(string key)
        {
            AudioClip clip = null;
            
            yield return AudioManagement.Instance.GetClip(key, audioClip => clip = audioClip);

            if (clip == null)
            {
                Debug.LogError("Audio Management not found clip at key: " + key + ",\n Source Audio PlaySound: " + gameObject.name);
                yield break;
            }
            
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    
        public void Play()
        {
            if (_audioSource.clip != null)
            {
                _audioSource.Play();
            }
        }
    
        public void Stop()
        {
            _audioSource.Stop();
        }
    
        public void Pause()
        {
            _audioSource.Pause();
        }
    
        public void UnPause()
        {
            _audioSource.UnPause();
        }
    
        private void Update()
        {
            HandleLoop();
        }
    
        private void HandleLoop()
        {
            if (Loop == false || _audioSource.clip == null)
            {
                return;
            }
            
            if (_audioSource.time >= _audioSource.clip.length)
            {
                _audioSource.time = 0;
                _audioSource.Play();
            }
        }
    }
}
