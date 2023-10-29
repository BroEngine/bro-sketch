using System.Diagnostics;
using Bro.Client;

namespace Bro.Client.UI
{
    public static class ExtendedButtonState
    {
        public static bool Enabled => !_disableTimer.IsRunning || _disableTimer.ElapsedMilliseconds > _disabledMaxTime;

        private static readonly EventObserver<ExtendedButtonEnableEvent> _enableObserver = new EventObserver<ExtendedButtonEnableEvent>();
        private static readonly EventObserver<ExtendedButtonDisableEvent> _disableObserver = new EventObserver<ExtendedButtonDisableEvent>();
        private static readonly Stopwatch _disableTimer = new Stopwatch();
        private static int _disabledMaxTime;
            
        static ExtendedButtonState()
        {
            _enableObserver.Subscribe(OnEnableEvent);   
            _disableObserver.Subscribe(OnDisableEvent);   
        }

        private static void OnDisableEvent(ExtendedButtonDisableEvent e)
        {
            _disabledMaxTime = (int) (e.MaxTime * 1000L);
            _disableTimer.Restart();
        } 
        
        private static void OnEnableEvent(ExtendedButtonEnableEvent e)
        {
            _disableTimer.Reset();
        }
    }
}