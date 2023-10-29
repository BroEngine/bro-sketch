using UnityEngine;

namespace Bro
{
    public class HapticEngine
    {
        #if UNITY_ANDROID
        private static AndroidJavaClass _unityPlayer;
        private static AndroidJavaObject _currentActivity;
        private static AndroidJavaObject _vibrator;
        private static AndroidJavaObject _context;
        private static AndroidJavaClass _vibrationEffect;
        private static int _androidVersion;
        #endif
        
        public HapticEngine ()
        {
            #if UNITY_ANDROID
            if (Application.isMobilePlatform)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    var androidVersion = SystemInfo.operatingSystem;
                    var sdkPos = androidVersion.IndexOf ( "API-" );
                    _androidVersion = int.Parse ( androidVersion.Substring ( sdkPos + 4, 2 ).ToString () );
                }
                
                _unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer" );
                _currentActivity = _unityPlayer.GetStatic<AndroidJavaObject>("currentActivity" );
                _vibrator = _currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator" );
                _context = _currentActivity.Call<AndroidJavaObject>("getApplicationContext" );

                if (_androidVersion >= 26)
                {
                    _vibrationEffect = new AndroidJavaClass ( "android.os.VibrationEffect" );
                }
            }
            #endif
        }
        
        public void Vibrate(long milliseconds) /* Android */
        {
            if (Application.isMobilePlatform)
            {
                #if UNITY_ANDROID
                if (_androidVersion >= 26)
                {
                    var createOneShot = _vibrationEffect.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, -1);
                    _vibrator.Call("vibrate", createOneShot);
                } 
                else 
                {
                    _vibrator.Call("vibrate", milliseconds);
                }
                #else
                Vibrate();
                #endif
            }
        }

        public void Vibrate()
        {
            if (Application.isMobilePlatform)
            {
                Handheld.Vibrate();
            }
        }
    }
}