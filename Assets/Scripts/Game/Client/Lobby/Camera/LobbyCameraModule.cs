using Bro.Client.Context;
using Cysharp.Threading.Tasks;
using Game.Client.Lobby.UI;
using UnityEngine;

namespace Game.Client.Lobby
{
    public class LobbyCameraModule : IClientContextModule
    {
        private IClientContext _context;
        
        private LobbyInterfaceCamera _interfaceCamera;
        
        public void Setup(IClientContext context)
        {
            _context = context;
        }
        
        public async UniTask Load()
        {
            _interfaceCamera = Object.FindObjectOfType<LobbyInterfaceCamera>(true);
        }
        
        public async UniTask Unload()
        {
            
        }
    }
}