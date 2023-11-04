using System.Collections.Generic;
using UnityEngine;

namespace Game.Client
{
    public class AudioBehaviour : MonoBehaviour
    {
        private const int PoolSize = 8;
        
        private readonly List< AudioSource > _sourceQueue = new List<AudioSource>();
        private AudioSource _sourceLoop;
        private AudioSource _sourceLong;
        private AudioSource _sourceAmbient;

        private int _queueIndex;
        private int _sourceIndex;
        
        public void SetGlobal()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Generate()
        {
            for (var i = 0; i < PoolSize; ++i)
            {
                _sourceQueue.Add(CreateSource(false, "common"));
            }

            _sourceLoop = CreateSource(true, "loop");
            _sourceLong = CreateSource(false, "long");
            _sourceAmbient = CreateSource(false, "ambient");
        }

        private AudioSource CreateSource(bool isLoop, string suffix)
        {
            var audioObject = new GameObject($"audio_source_{_sourceIndex:00}_{suffix}");
            var audioSource = audioObject.AddComponent<AudioSource>();
            audioObject.transform.parent = transform;
            audioSource.loop = isLoop;
            ++_sourceIndex;
            return audioSource;
        }

        public void Setup(bool musicOn, bool soundOn)
        {
            AudioListener.pause = !soundOn;
            _sourceAmbient.ignoreListenerPause = musicOn;
            _sourceAmbient.mute = !musicOn;
        }

        public void SimplePlay (AudioClip clip, float volume)
        {
            PlayInQueue(clip, volume);
        }

        public void LoopPlay(AudioClip clip, float volume)
        {
            _sourceLoop.clip = clip;
            _sourceLoop.volume = volume;
            _sourceLoop.loop = true;
            _sourceLoop.Play();
        }

        public void LoopStop()
        {
            _sourceLoop.loop = false;
            _sourceLoop.Stop();
        }

        public void LongPlay(AudioClip clip, float volume)
        {
            _sourceLong.clip = clip;
            _sourceLong.volume = volume;
            _sourceLong.loop = false;
            _sourceLong.Play();
        }

        public void AmbientPlay(AudioClip clip, float volume)
        {
            _sourceAmbient.clip = clip;
            _sourceAmbient.volume = volume;
            _sourceAmbient.loop = true;
            _sourceAmbient.Play();
        }

        private void PlayInQueue(AudioClip clip, float volume, bool loop = false)
        {
            ++_queueIndex;

            if (_queueIndex >= _sourceQueue.Count)
            {
                _queueIndex = 0;
            }

            if (_queueIndex >= _sourceQueue.Count)
            {
                return;
            }

            var queue = _sourceQueue[_queueIndex];
            
            if (queue == null)
            {
                return; 
            }
            
            _sourceQueue[_queueIndex].clip = clip;
            _sourceQueue[_queueIndex].volume = volume;
            _sourceQueue[_queueIndex].loop = loop;
            _sourceQueue[_queueIndex].Play();
        }
    }
}