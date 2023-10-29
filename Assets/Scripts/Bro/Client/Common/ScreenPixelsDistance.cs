using UnityEngine.Device;

namespace Bro.Client
{
    public class ScreenPixelsDistance
    {
        private bool _isInitialized;
        private float _value;
        private readonly float _valueInCentimeters;

        public float Value
        {
            get
            {
                if (!_isInitialized)
                {
                    _value = _valueInCentimeters * Screen.dpi / 2.54f;
                }

                return _value;
            }
        }

        public ScreenPixelsDistance(float distanceInCentimeters)
        {
            _valueInCentimeters = distanceInCentimeters;
        }
    }
}