using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Windows
{
    public class YouWinWindow : Window
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _goToMenuButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _restartButton;

        [SerializeField] private Image _playerAvatar;
        [SerializeField] private TextMeshProUGUI _currentScoreText;
        [SerializeField] private TextMeshProUGUI _maxScoreText;

        [SerializeField] private List<Image> _stars;
        [SerializeField] private Sprite _filledStarSprite;
        [SerializeField] private Sprite _emptyStarSprite;


        private EventBus _eventBus;

        void Start()
        {
            _nextLevelButton.onClick.AddListener(NextLevel);
            _goToMenuButton.onClick.AddListener(GoToMenu);
            _settingsButton.onClick.AddListener(OnSettings);
            _restartButton.onClick.AddListener(OnRestartLevel);

            _eventBus = ServiceLocator.Current.Get<EventBus>();

        }

        public void Init(int currentScore, int countStars)
        {
            _currentScoreText.text = "Score " + currentScore.ToString(); 
            _maxScoreText.text = "Best score: " + PlayerPrefs.GetInt(StringConstants.MAX_SCORE, 0).ToString();

            _playerAvatar.sprite = UserProfile.Utils.SpriteSaver.LoadSprite(StringConstants.ACCOUNT_AVATAR_FILENAME) ?? _playerAvatar.sprite;


            for (int i = 0; i < _stars.Count; i++)
            {
                if (i < countStars)
                {
                    _stars[i].sprite = _filledStarSprite;
                }
                else
                {
                    _stars[i].sprite = _emptyStarSprite;
                }
            }
        }

        private void NextLevel()
        {
            Hide();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
            _eventBus.Invoke(new NextLevelSignal());
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
            _nextLevelButton.onClick.RemoveAllListeners();
            _goToMenuButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
            _restartButton.onClick.RemoveAllListeners();
        }
    }
}
