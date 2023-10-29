using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Bro.Client.UI
{
    public class WindowRapidTweenAnimator : TweenAnimator
    {
        public const float Time = 0.1f;
        
        private Image _backgroundImage;
        private Color _backgroundColor;
        private Transform _window;
        private readonly List<Transform> _elements = new List<Transform>();
        private bool _isSetup;
        
        private void Setup()
        {
            if (!_isSetup)
            {
                _isSetup = true;
                foreach (Transform t in transform)
                {
                    if (t.name.ToLower().Contains("window"))
                    {
                        _window = t;
                    }
                    
                    if (t.name.ToLower().Contains("element"))
                    {
                        _elements.Add(t);
                    }

                    if (t.name.ToLower().Contains("background"))
                    {
                        _backgroundImage = t.GetComponent<Image>();
                        _backgroundColor = _backgroundImage.color;
                    }
                 
                }
                
                Debug.Assert(_backgroundImage != null, $"{name} has no background named image");
                Debug.Assert(_window != null, $"{name} has no window named object");
            }
        }

        public override async UniTask Show()
        {
            Setup();

            foreach (var e in _elements)
            {
                e.localScale = Vector3.zero;
            }
            
            _window.localScale = Vector3.zero;
            _backgroundImage.color = Color.clear;
            _backgroundImage.DOColor(_backgroundColor, Time).SetEase(Ease.InOutSine);

            foreach (var e in _elements)
            {
                e.DOScale(Vector3.one, Time).SetEase(Ease.OutBack);
            }
            
            await _window.DOScale(Vector3.one, Time).SetEase(Ease.OutBack);
        }
        
        public override async UniTask Hide()
        {
            foreach (var e in _elements)
            {
                e.DOScale(Vector3.zero, Time).SetEase(Ease.InBack);
            }
            
            _backgroundImage.DOColor(Color.clear, Time).SetEase(Ease.InOutSine);
            await _window.DOScale(Vector3.zero, Time).SetEase(Ease.InBack);
        }
    }
}