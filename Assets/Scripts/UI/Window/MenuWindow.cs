using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Windows
{
    public class MenuWindow : Window
    {
        [SerializeField] private Button _profileButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private Button _howToPlayButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _privacyPolicyButton;
        [SerializeField] private Button _termsOfUseButton;

        [SerializeField] private Image _profileAvatar;
        [SerializeField] private TextMeshProUGUI _profileName;

        protected void Awake()
        {
            _profileButton.onClick.AddListener(OnProfileButtonClick);
            _settingsButton.onClick.AddListener(OnSettingsButtonClick);
            _leaderboardButton.onClick.AddListener(OnLeaderboardButtonClick);
            _howToPlayButton.onClick.AddListener(OnHowToPlayButtonClick);
            _playButton.onClick.AddListener(OnPlayButtonClick);
            _privacyPolicyButton.onClick.AddListener(OnPrivacyPolicyButtonClick);
            _termsOfUseButton.onClick.AddListener(OnTermsOfUseButtonClick);

            _profileAvatar.sprite = UserProfile.Utils.SpriteSaver.LoadSprite(StringConstants.ACCOUNT_AVATAR_FILENAME) ?? _profileAvatar.sprite;
            _profileName.text = "Hello, " + PlayerPrefs.GetString(StringConstants.ACCOUNT_NAME, "Username");
        }
        private void OnProfileButtonClick()
        {
            Hide();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
            WindowManager.ShowWindow<ProfileWindow>();
        }
        private void OnSettingsButtonClick()
        {
            Hide();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
            SettingsWindow settingsWindow = WindowManager.ShowWindow<SettingsWindow>();
            settingsWindow.Init(this);
        }
        private void OnLeaderboardButtonClick()
        {
            Hide();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
            WindowManager.ShowWindow<LeaderboardWindow>();
            
        }
        private void OnHowToPlayButtonClick()
        {
            Hide();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
            WindowManager.ShowWindow<HowToPlayWindow>();
        }
        private void OnPlayButtonClick()
        {
            Hide();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
            WindowManager.ShowWindow<SelectLevelWindow>();
        }
        private void OnPrivacyPolicyButtonClick()
        {
            Hide();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
            WindowManager.ShowWindow<PrivacyPolicyWindow>();
        }
        private void OnTermsOfUseButtonClick()
        {
            Hide();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
            WindowManager.ShowWindow<TermsOfUseWindow>();
        }

        private void OnDestroy()
        {
            _profileButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
            _leaderboardButton.onClick.RemoveAllListeners();
            _howToPlayButton.onClick.RemoveAllListeners();
            _playButton.onClick.RemoveAllListeners();
            _privacyPolicyButton.onClick.RemoveAllListeners();
            _termsOfUseButton.onClick.RemoveAllListeners();
        }
    }
}
