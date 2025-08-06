using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Windows
{
    public class YouLoseWindow : Window
    {
        [SerializeField] private Button _goToMenuButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _restartButton;

        [SerializeField] private Image _playerAvatar;
        [SerializeField] private TextMeshProUGUI _currentScoreText;
        [SerializeField] private TextMeshProUGUI _maxScoreText;


        private EventBus _eventBus;

        void Start()
        {
            _goToMenuButton.onClick.AddListener(GoToMenu);
            _settingsButton.onClick.AddListener(OnSettings);
            _restartButton.onClick.AddListener(OnRestartLevel);

            _eventBus = ServiceLocator.Current.Get<EventBus>();

        }

        public void Init(int currentScore)
        {
            _currentScoreText.text = "Score " + currentScore.ToString(); 
            _maxScoreText.text = "Best score: " + PlayerPrefs.GetInt(StringConstants.MAX_SCORE, 0).ToString();

            _playerAvatar.sprite = UserProfile.Utils.SpriteSaver.LoadSprite(StringConstants.ACCOUNT_AVATAR_FILENAME) ?? _playerAvatar.sprite;
        }

        private void GoToMenu()
        {
            Hide();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
            SceneManager.LoadScene(StringConstants.MENU_SCENE);
        }

        private void OnSettings()
        {
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
            WindowManager.ShowWindow<SettingsWindow>();
        }

        private void OnRestartLevel()
        {
            _eventBus.Invoke(new RestartLevelSignal());
            Hide();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
        }

        private void OnDestroy()
        {
            _goToMenuButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
            _restartButton.onClick.RemoveAllListeners();
        }
    }
}
