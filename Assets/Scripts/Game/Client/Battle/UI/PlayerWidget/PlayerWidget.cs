using Bro.Client.Context;
using UnityEngine;

namespace Game.Client.Battle.UI
{
    public class PlayerWidget : MonoBehaviour
    {
        [SerializeField] private TrackerWidget _trackerWidget;
        
        private IClientContext _context;
        private HeroModule _heroModule;
        
        public void Setup(IClientContext context)
        {
            _context = context;
            _heroModule = _context.Get<HeroModule>();
            
            var vehicle = _heroModule.Vehicle;
            _trackerWidget.SetupTrack(vehicle.transform);
        }

        private void LateUpdate()
        {
            UpdateData();
        }

        private void UpdateData()
        {

        }
    }
}