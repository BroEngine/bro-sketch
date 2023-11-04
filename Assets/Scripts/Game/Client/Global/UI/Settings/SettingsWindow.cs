using Bro.Client;
using Bro.Client.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Client.UI
{
    [Window(WindowItemType.Popup)]
    public class SettingsWindow : Window
    {
        [SerializeField] private Button _buttonMusicOn;
        [SerializeField] private Button _buttonMusicOff;
        [SerializeField] private Button _buttonSoundOn;
        [SerializeField] private Button _buttonSoundOff;
        [SerializeField] private Button _buttonLanguage;
        [SerializeField] private Button _buttonPrivacy;
        [SerializeField] private Button _buttonMail;
        
        private LocalProfileModule _profile;
        private AudioModule _audio;
        private SettingsData _settings;
        private UIModule _ui;

        private void Awake()
        {
            _buttonMusicOn.onClick.AddListener(OnButtonMusicOn);
            _buttonMusicOff.onClick.AddListener(OnButtonMusicOff);
            _buttonSoundOn.onClick.AddListener(OnButtonSoundOn);
            _buttonSoundOff.onClick.AddListener(OnButtonSoundOff);
            _buttonLanguage.onClick.AddListener(OnButtonLanguage);
            _buttonPrivacy.onClick.AddListener(OnButtonPrivacy);
            _buttonMail.onClick.AddListener(OnButtonMail);
        }

        protected override void OnShow(IWindowArgs args)
        {
            _ui = _context.Get<UIModule>();
            _profile = _context.GetGlobal<LocalProfileModule>();
            _audio = _context.GetGlobal<AudioModule>();
            _settings = _profile.Settings;
            
            ReDraw();
        }

        private void ReDraw()
        {
            _buttonMusicOff.gameObject.SetActive(!_settings.MusicEnabled);
            _buttonMusicOn.gameObject.SetActive(_settings.MusicEnabled);
            
            _buttonSoundOff.gameObject.SetActive(!_settings.SoundEnabled);
            _buttonSoundOn.gameObject.SetActive(_settings.SoundEnabled);
        }

        private void OnButtonClose()
        {
            DirectlyHide();
        }

        private void OnButtonMusicOn()
        {
            _settings.ToggleMusic();
            _audio.UpdateSettings();
            ReDraw();
        } 
        
        private void OnButtonMusicOff()
        {
            _settings.ToggleMusic();
            _audio.UpdateSettings();
            ReDraw();
        }  
        
        private void OnButtonSoundOn()
        {
            _settings.ToggleSound();
            _audio.UpdateSettings();
            ReDraw();
        }
        
        private void OnButtonSoundOff()
        {
            _settings.ToggleSound();
            _audio.UpdateSettings();
            ReDraw();
        }

        private void OnButtonLanguage()
        {
            OnButtonClose();
            _ui.Show<LanguageWindow>();
        }

        private void OnButtonPrivacy()
        {
            Application.OpenURL(Config.PrivacyPolicy);
        }
        
        private void OnButtonMail()
        {
            var subject = System.Uri.EscapeUriString(Config.SupportTitle);
            var title = System.Uri.EscapeUriString(LogMonitor.GetPrefix());
            Application.OpenURL("mailto:" + Config.SupportEmail + "?subject=" + subject + "&body=" + title);  
        }
    }
}