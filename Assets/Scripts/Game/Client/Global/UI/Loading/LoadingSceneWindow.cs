using Bro.Client.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Client.UI
{
    [Window(WindowItemType.Widget)]
    public class LoadingSceneWindow : Window
    {
        [SerializeField] private TextMeshProUGUI _labelVersion;
        [SerializeField] private Slider _sliderProgress;
        
        private Tween _tween;
        
        protected override void OnShow(IWindowArgs args)
        {
            var version = AssemblyInfo.Version;
            _labelVersion.text = $"v{version}";
            _tween?.Kill();
            _sliderProgress.value = 0.0f;
        }
        
        public void UpdateProgress(float ratio, float duration)
        {
            _tween?.Kill();
            _tween = _sliderProgress.DOValue(ratio, duration);
        }
        
        public void UpdateProgress(float ratio)
        {
            var duration = 0.25f;
            _tween?.Kill();
            _tween = _sliderProgress.DOValue(ratio, duration);
        }
    }
}