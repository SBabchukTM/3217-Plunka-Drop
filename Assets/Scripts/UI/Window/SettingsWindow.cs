using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CustomEventBus;
using CustomEventBus.Signals;
using System.IO;
using UserProfile.Utils;
using System;

namespace UI.Windows
{
    public class SettingsWindow : Window
    {
        [SerializeField] private Button _notificationButton;
        [SerializeField] private Button _musicButton;
        [SerializeField] private Button _vibrationButton;
        [SerializeField] private Button _soundButton;

        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _backButton;

        [SerializeField] private Sprite _notificationOnSprite;
        [SerializeField] private Sprite _notificationOffSprite;
        [SerializeField] private Sprite _musicOnSprite;
        [SerializeField] private Sprite _musicOffSprite;
        [SerializeField] private Sprite _vibrationOnSprite;
        [SerializeField] private Sprite _vibrationOffSprite;
        [SerializeField] private Sprite _soundOnSprite;
        [SerializeField] private Sprite _soundOffSprite;

        [SerializeField] private Image _backgroundBlocker;

        private EventBus _eventBus;

        private Type _previousWindowType;

        private bool _notificationsEnabled;
        private bool _musicEnabled;
        private bool _vibrationEnabled;
        private bool _soundEnabled;

        private void Start()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();

            LoadSettings();

            _notificationButton.onClick.AddListener(ToggleNotifications);
            _musicButton.onClick.AddListener(ToggleMusic);
            _vibrationButton.onClick.AddListener(ToggleVibration);
            _soundButton.onClick.AddListener(ToggleSound);
            _saveButton.onClick.AddListener(SaveSettings);
            _backButton.onClick.AddListener(OnBack);

            if (_previousWindowType == null)
            {
                _backgroundBlocker.gameObject.SetActive(true);
            }
        }

        public void Init(Window previousWindow)
        {
            _previousWindowType = previousWindow.GetType();

            _backgroundBlocker.gameObject.SetActive(false);
        }

        private void LoadSettings()
        {
            _notificationsEnabled = PlayerPrefs.GetInt(StringConstants.NOTIFICATIONS_KEY, 1) == 1;
            _musicEnabled = PlayerPrefs.GetInt(StringConstants.MUSIC_KEY, 1) == 1;
            _vibrationEnabled = PlayerPrefs.GetInt(StringConstants.VIBRATION_KEY, 1) == 1;
            _soundEnabled = PlayerPrefs.GetInt(StringConstants.SOUND_KEY, 1) == 1;

            UpdateIcons();
        }

        private void SaveSettings()
        {
            PlayerPrefs.SetInt(StringConstants.NOTIFICATIONS_KEY, _notificationsEnabled ? 1 : 0);
            PlayerPrefs.SetInt(StringConstants.MUSIC_KEY, _musicEnabled ? 1 : 0);
            PlayerPrefs.SetInt(StringConstants.VIBRATION_KEY, _vibrationEnabled ? 1 : 0);
            PlayerPrefs.SetInt(StringConstants.SOUND_KEY, _soundEnabled ? 1 : 0);

            _eventBus.Invoke(new SettingsChangedSignal());
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();

            _saveButton.interactable = false;
        }

        private void UpdateIcons()
        {
            _notificationButton.image.sprite = _notificationsEnabled ? _notificationOnSprite : _notificationOffSprite;
            _musicButton.image.sprite = _musicEnabled ? _musicOnSprite : _musicOffSprite;
            _vibrationButton.image.sprite = _vibrationEnabled ? _vibrationOnSprite : _vibrationOffSprite;
            _soundButton.image.sprite = _soundEnabled ? _soundOnSprite : _soundOffSprite;
        }

        private void ToggleNotifications()
        {
            _notificationsEnabled = !_notificationsEnabled;
            UpdateIcons();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();

            _saveButton.interactable = true;
        }

        private void ToggleMusic()
        {
            _musicEnabled = !_musicEnabled;
            UpdateIcons();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();

            _saveButton.interactable = true;
        }

        private void ToggleVibration()
        {
            _vibrationEnabled = !_vibrationEnabled;
            UpdateIcons();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();

            _saveButton.interactable = true;
        }

        private void ToggleSound()
        {
            _soundEnabled = !_soundEnabled;
            UpdateIcons();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();

            _saveButton.interactable = true;
        }

        private void OnBack()
        {
            Hide();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
            if (_previousWindowType != null)
                WindowManager.ShowWindowByType(_previousWindowType);

            _backgroundBlocker.gameObject.SetActive(false);
        }


        private void OnDestroy()
        {
            _notificationButton.onClick.RemoveAllListeners();
            _musicButton.onClick.RemoveAllListeners();
            _vibrationButton.onClick.RemoveAllListeners();
            _soundButton.onClick.RemoveAllListeners();
            _saveButton.onClick.RemoveAllListeners();
            _backButton.onClick.RemoveAllListeners();
        }
    }
}
