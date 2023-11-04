using NaughtyAttributes;
using UnityEngine;

namespace Game.Client.Lobby.UI
{
    public class LobbyInterfaceCamera : MonoBehaviour, ILobbyCamera
    {
        [SerializeField] [Required] private Camera _camera;
        
        public void Enable()
        {
            _camera.enabled = true;
        }
        
        public void Disable()
        {
            _camera.enabled = false;
        }
    }
}