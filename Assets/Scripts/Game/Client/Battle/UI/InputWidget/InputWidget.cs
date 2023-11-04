using Bro.Client.Context;
using Game.Client.Battle.Input;
using UnityEngine;

namespace Game.Client.Battle.UI
{
    public class InputWidget : MonoBehaviour
    {
        private IClientContext _context;
        private InputModule _inputModule;

        private void Awake()
        {
               
        }

        public void Setup(IClientContext context)
        {
            _context = context;
            _inputModule = _context.Get<InputModule>();
        }

        private void Update()
        {
            
        }
    }
}