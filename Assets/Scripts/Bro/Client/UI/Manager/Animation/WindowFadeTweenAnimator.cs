using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Bro.Client.UI
{
    public class WindowFadeTweenAnimator : TweenAnimator
    {
        /*
         window - canvas
         background - image - optional
         */
        
        private Image _backgroundImage;
        private Color _backgroundColor;
        private CanvasGroup _window;
        private bool _isSetup;

        [SerializeField] private float _timeFade = 0.3f;
        [SerializeField] private float _timeBackground = 0.1f;
        
        private void Setup()
        {
            if (!_isSetup)
            {
                _isSetup = true;
                foreach (Transform t in transform)
                {
                    if (t.name.ToLower().Contains("window"))
                    {
                        if (t.GetComponent<CanvasGroup>())
                        {
                            _window = t.GetComponent<CanvasGroup>();
                        }
                    }
                    
                    if (t.name.ToLower().Contains("background"))
                    {
                        _backgroundImage = t.GetComponent<Image>();
                        _backgroundColor = _backgroundImage.color;
                    }
                }
                
                Debug.Assert(_window != null, $"{name} has no window named object");
            }
        }

        public override async UniTask Show()
        {
            Setup();

            _window.alpha = 0.0f;
            if (_backgroundImage != null)
            {
                _backgroundImage.color = Color.clear;
                await _backgroundImage.DOColor(_backgroundColor, _timeBackground).SetEase(Ease.InOutSine);
            }
            await _window.DOFade(1.0f, _timeFade).SetEase(Ease.InOutSine);
        }
        
        public override async UniTask Hide()
        {
            await _window.DOFade(0.0f, _timeFade).SetEase(Ease.InOutSine);

            if (_backgroundImage != null)
            {
                await _backgroundImage.DOColor(Color.clear, _timeBackground).SetEase(Ease.InOutSine);
            }
        }
    }
}