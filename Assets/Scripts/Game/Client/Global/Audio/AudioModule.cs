using System;
using Bro;
using Bro.Client.Context;
using Bro.Client.UI;
using Game.Client.Battle;
using Game.Client.Lobby;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Game.Client
{
    public class AudioModule : IClientContextModule
    {
        public AudioBehaviour Audio => _audio;
        
        private AudioBehaviour _audio;
        private IClientContext _context;
        private AudioRegistry _registry;
        private SettingsData _settings;
        private AudioType _currentLoop;
        private bool _isAmbientStarted = false;
        
        public void Setup(IClientContext context)
        {
            _context = context;
        }
        
        public async UniTask Load()
        {
            _registry = AudioRegistry.Instance;
            _settings = _context.GetGlobal<LocalProfileModule>().Settings;

            _audio = Create();
            
            _context.AddDisposable(new EventObserver<BattleLoadedEvent>(OnBattleLoadedEvent));
            _context.AddDisposable(new EventObserver<LobbyLoadedEvent>(OnLobbyLoadedEvent));
            _context.AddDisposable(new EventObserver<AudioEvent>(OnAudioEvent));
            _context.AddDisposable(new EventObserver<UIAudioEvent>(OnUIAudioEvent));
        }
        
        public async UniTask Unload()
        {
            
        }

        private void OnBattleLoadedEvent(BattleLoadedEvent e)
        {
            AmbientPlay(AudioType.MusicBattle);
        }

        private void OnLobbyLoadedEvent(LobbyLoadedEvent e)
        {
            AmbientPlay(AudioType.MusicLobby);
        }
        
        private void OnAudioEvent(AudioEvent e)
        {
            if (e.IsLoop)
            {
                LoopPlay(e.AudioType);
            }
            else
            {
                SimplePlay(e.AudioType);
            }
        }

        private void OnUIAudioEvent(UIAudioEvent e)
        {
            switch (e.Type)
            {
                case UIAudioType.WindowShow:
                    // SimplePlay(AudioType.SoundUIWindowShow);
                    break;
                case UIAudioType.WindowHide:
                    // SimplePlay(AudioType.SoundUIWindowHide);
                    break;
                case UIAudioType.ButtonPress:
                    // SimplePlay(AudioType.SoundUIButtonPress);
                    break;
                case UIAudioType.ButtonClose:
                    // SimplePlay(AudioType.SoundUIButtonPress);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void AmbientPlay(AudioType type)
        {
            _audio.AmbientPlay(_registry.GetClip(type), _registry.GetVolume(type));
        }
        
        private void SimplePlay(AudioType type)
        {
            _audio.SimplePlay(_registry.GetClip(type), _registry.GetVolume(type));
        }

        private void LoopPlay(AudioType type)
        {
            if (_currentLoop == type)
            {
                _currentLoop = 0;
                _audio.LoopStop();
            }
            else
            {
                _currentLoop = type;
                _audio.LoopPlay(_registry.GetClip(type), _registry.GetVolume(type));   
            }
        }

        public void UpdateSettings()
        {
            _audio.Setup(_settings.MusicEnabled, _settings.SoundEnabled);
        }

        private AudioBehaviour Create()
        {
            var behaviour = new GameObject("audio").AddComponent<AudioBehaviour>();
            behaviour.Generate();
            behaviour.SetGlobal();
            return behaviour;
        }
    }
}