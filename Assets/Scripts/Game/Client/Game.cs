using System;
using System.Collections.Generic;
using Bro.Client;
using Bro.Client.Context;
using Game.Client.Entry;
using Game.Client.Lobby;
using Game.Client.Battle;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Client
{
    public class Game : MonoBehaviour, IApplication
    {
        public static IApplication Instance { get; private set; } /* for special situation */

        private readonly Dictionary<Type, string> _scenes = new Dictionary<Type, string>()
        {
            {typeof(EntryContext), "scene_entry"},
            {typeof(LobbyContext), "scene_lobby"},
            {typeof(BattleContext), "scene_battle"}
        };
        
        private IClientContext _localContext;
        private IClientContext _globalContext;
        private UnityScheduler _globalScheduler;
        private UnityScheduler _localScheduler;
        
        IClientContext IApplication.LocalContext => _localContext;
        IClientContext IApplication.GlobalContext => _globalContext;
        
        private async void Awake()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            LogMonitor.Initialize();
            
            Instance = this;
            
            _globalScheduler = new GameObject("global_scheduler").AddComponent<UnityScheduler>();
            _localScheduler = new GameObject("local_scheduler").AddComponent<UnityScheduler>();
            
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(_globalScheduler);
            DontDestroyOnLoad(_localScheduler);
            
            _globalContext = new GlobalContext(this);
            _localContext = new EntryContext(this);
            
            await _globalContext.Load();
            await _localContext.Load();
        }

        private void OnDestroy()
        {
            _globalContext.Unload();
            _localContext.Unload();
            Instance = null;
        }

        IScheduler IApplication.GetScheduler(IClientContext context)
        {
            if (context == _globalContext)
            {
                return _globalScheduler;
            }
            return _localScheduler;
        }
        
        public async UniTask SwitchToLobby()
        {
            var context = new LobbyContext(this);
            await SwitchContext(context);
        }

        public async UniTask SwitchToBattle(BattleConfig config)
        {
            var context = new BattleContext(this, config);
            await SwitchContext(context);
        }
        
        public async UniTask SwitchContext(ClientContext context)
        {
            var newSceneName = _scenes[context.GetType()]; // todo
            var oldSceneName = _scenes[_localContext.GetType()]; // todo
            
            await _localContext.Unload();
            
            await SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);
            await SceneManager.UnloadSceneAsync(oldSceneName);
            
            _localContext = context;
            await _localContext.Load();
        }
    }
}