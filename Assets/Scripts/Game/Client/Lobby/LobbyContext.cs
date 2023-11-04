using Bro.Client.Context;
using Bro.Client.UI;

namespace Game.Client.Lobby
{
    public class LobbyContext : ClientContext
    {
        private readonly UIModule _uiModule = new UIModule(false, false);
        private readonly LobbyCameraModule _lobbyCameraModule = new LobbyCameraModule();
        private readonly LobbyLifecycleModule _lobbyLifecycleModule = new LobbyLifecycleModule();
        
        public LobbyContext(IApplication application) : base(application)
        {
         
        }
    }
}