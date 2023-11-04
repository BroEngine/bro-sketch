using System;
using Bro.Client.Context;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Client.Battle.Input
{
    public class InputModule : IClientContextModule
    {
        private IClientContext _context;
        private SettingsModule _settings;
       
        public void Setup(IClientContext context)
        {
            _context = context;
            _settings = _context.Get<SettingsModule>();
        }
        
        public async UniTask Load()
        {
            _context.AddDisposable(_context.Scheduler.ScheduleUpdate(OnUpdate));
            _context.AddDisposable(_context.Scheduler.ScheduleLateUpdate(OnLateUpdate));
        }
        
        public async UniTask Unload()
        {
          
        }

        public void Reset()
        {
            
        }

        private void OnUpdate(float dt)
        {
          
        }

        private void OnLateUpdate(float dt)
        {
          
        }
    }
}