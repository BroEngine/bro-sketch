using System.Diagnostics;

namespace Bro
{
    public static class HapticManager
    {
        private static int StrengthWeak = 14;
        private static int StrengthMedium = 25;
        private static int StrengthStrong = 50;

        private const int CooldownCommon = 100; // ms
        
        private static readonly HapticEngine _engine;

        private static readonly Stopwatch _timerCustom = new Stopwatch();
        private static readonly Stopwatch _timerWeak = new Stopwatch();
        private static readonly Stopwatch _timerMedium = new Stopwatch();
        private static readonly Stopwatch _timerStrong = new Stopwatch();

        private static bool _isEnabled = true;
        
        static HapticManager()
        {
            _engine = new HapticEngine();
        }

        public static void Vibrate(int strength)
        {
            if (_isEnabled)
            {
                _engine.Vibrate(strength);
            }
        } 

        public static void VibrateWeak()
        {
            if (_isEnabled)
            {
                _engine.Vibrate(StrengthWeak);
            }
        } 
        
        public static void VibrateWeakWithCooldown(int cooldown = CooldownCommon)
        {
            VibrateCooldown(StrengthWeak, CooldownCommon, _timerWeak);
        }
        
        public static void VibrateMedium()
        {
            if (_isEnabled)
            {
                _engine.Vibrate(StrengthMedium);
            }
        } 
        
        public static void VibrateMediumWithCooldown(int cooldown = CooldownCommon)
        {
            VibrateCooldown(StrengthMedium, CooldownCommon, _timerMedium);
        }
        
        public static void VibrateStrong()
        {
            if (_isEnabled)
            {
                _engine.Vibrate(StrengthMedium);
            }
        } 
        
        public static void VibrateStrongWithCooldown(int cooldown = CooldownCommon)
        {
            VibrateCooldown(StrengthStrong, CooldownCommon, _timerStrong);
        }

        private static void VibrateCooldown(int strength, int cooldown, Stopwatch timer)
        {
            if (!timer.IsRunning || timer.ElapsedMilliseconds >= cooldown)
            {
                if (_isEnabled)
                {
                    _engine.Vibrate(strength);
                }
                timer.Restart();
            }
        }
        
        public static void SetEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;
        }

        public static void SetStrengthWeak(int value)
        {
            StrengthWeak = value;
        }
        
        public static void SetStrengthMedium(int value)
        {
            StrengthMedium = value;
        }
        
        public static void SetStrengthString(int value)
        {
            StrengthStrong = value;
        }
    }
}