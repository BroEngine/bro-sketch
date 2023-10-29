using System;
using TMPro;

namespace Bro.Client.UI
{
    public class LabelValueIntChanger
    {
        private readonly TextMeshProUGUI _label;
        private readonly string _mask;
        private int _value;
        public event Action<int> OnChange;
        
        public LabelValueIntChanger(TextMeshProUGUI label, int defaultValue = 0, string mask = "")
        {
            _label = label;
            _mask = mask;
            _value = defaultValue;
            UpdateText();
        }
        
        public void UpdateText()
        {
            if (string.IsNullOrEmpty(_mask))
            {
                _label.text = _value.ToString();
            }
            else
            {
                _label.text = string.Format(_mask, _value);
            }
            OnChange?.Invoke(_value);
        }

        public bool Set(int newValue)
        {
            if (_value != newValue)
            {
                _value = newValue;
                UpdateText();

                return true;
            }

            return false;
        }
    }
}