using System;
using Bro.Client.Configs;
using Bro.Client.Context;
using Bro.Client.UI;
using Game.Client.Battle;
using Game.Client.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Client.Entry
{
    public class LoadingModule : IClientContextModule
    {
        private IClientContext _context;
        
        void IClientContextElement.Setup(IClientContext context)
        {
            _context = context;
        }
        
        async UniTask IClientContextModule.Load()
        {
            var isDevelopment = _context.IsDevelopment();
            var uiModule = _context.GetGlobal<UIModule>();
            var configModule = _context.GetGlobal<ConfigModule>();
            var privacyPolicy = new PrivacyPolicyCheck(_context);
            
            await uiModule.AwaitShow<LoadingSceneWindow>();
            var window = uiModule.GetWindow<LoadingSceneWindow>();
            
            window.UpdateProgress(0.2f);
            
            await privacyPolicy.Process();
            window.UpdateProgress(0.4f);
            
            await LoadConfigs<VehicleConfigStorage>(configModule);
            window.UpdateProgress(0.6f);
            
            
            window.UpdateProgress(1.0f);
            
            if (isDevelopment && DebugSettings.Instance.IsLoadCustomLevel)
            {
                Debug.LogError($"development: load custom battle id = {DebugSettings.Instance.CustomLevelId}");
                await ProcessBattle(DebugSettings.Instance.CustomLevelId);
            }
            else
            {
                await ProcessLobby();
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
                uiModule.DirectlyHide<LoadingSceneWindow>();
            }
        }

        private async UniTask ProcessBattle(string levelId)
        {
            var config = new BattleConfig();
            _context.GetGlobal<BattleSwitchingModule>().LoadRound(config);
        }
        
        private async UniTask ProcessLobby()
        {
            await _context.GetApplication().SwitchToLobby();   
        }
        
        public async UniTask Unload()
        {
           
        }

        private static async UniTask LoadConfigs<T>(ConfigModule configModule) where T : IConfigStorage, new()
        {
            await configModule.LoadConfigAsync(new ConfigStorageDescription()
            {
                Creator = () => new T(),
                Identifier = ConfigPath.GetIdentifier<T>(),
                LocalStoragePath = ConfigPath.GetConfigPath<T>(),
            });
        }
    }
}