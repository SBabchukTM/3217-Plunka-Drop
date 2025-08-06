using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CustomEventBus;
using CustomEventBus.Signals;
using System.IO;
using System.Linq;
using UserProfile.Utils;

namespace UI.Windows
{
    public class ProfileWindow : Window
    {
        [SerializeField] private TMP_InputField _usernameInput;
        [SerializeField] private TMP_InputField _emailInput;

        [SerializeField] private Image _avatarImage;
        [SerializeField] private Sprite _defaultAvatar;

        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _editUsernameButton;
        [SerializeField] private Button _editEmailButton;
        [SerializeField] private Button _editAvatarButton;
        [SerializeField] private Button _backButton;

        private EventBus _eventBus;

        private string _originalUsername;
        private string _originalEmail;
        private Sprite _originalAvatar;

        private void Start()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _eventBus.Subscribe<AvatarChangedSignal>(OnAvatarChanged);

            _usernameInput.onValueChanged.AddListener(OnUsernameChanged);
            _emailInput.onValueChanged.AddListener(OnEmailChanged);

            _saveButton.onClick.AddListener(SaveProfile);
            _editUsernameButton.onClick.AddListener(EnableUsernameEditing);
            _editEmailButton.onClick.AddListener(EnableEmailEditing);
            _editAvatarButton.onClick.AddListener(ChooseAvatar);
            _backButton.onClick.AddListener(OnBack);

            LoadProfile();
        }

        private void LoadProfile()
        {
            _originalUsername = PlayerPrefs.GetString(StringConstants.ACCOUNT_NAME, "Username");
            _originalEmail = PlayerPrefs.GetString(StringConstants.ACCOUNT_EMAIL, "Email");

            _usernameInput.text = _originalUsername;
            _emailInput.text = _originalEmail;

            _originalAvatar = SpriteSaver.LoadSprite(StringConstants.ACCOUNT_AVATAR_FILENAME);
            _avatarImage.sprite = _originalAvatar != null ? _originalAvatar : _defaultAvatar;
        }

        private void EnableUsernameEditing()
        {
            _usernameInput.interactable = true;
            _usernameInput.ActivateInputField();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
        }

        private void EnableEmailEditing()
        {
            _emailInput.interactable = true;
            _emailInput.ActivateInputField();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
        }

        private void OnUsernameChanged(string newValue)
        {
            CheckForChanges();
        }

        private void OnEmailChanged(string newValue)
        {
            CheckForChanges();
        }

        private void OnAvatarChanged(AvatarChangedSignal signal)
        {
            _avatarImage.sprite = signal.Avatar != null ? signal.Avatar : _defaultAvatar;
            CheckForChanges();
        }

        private void CheckForChanges()
        {
            bool isNameChanged = _usernameInput.text != _originalUsername;
            bool isEmailChanged = _emailInput.text != _originalEmail;
            bool isAvatarChanged = !IsAvatarSame();

            _saveButton.interactable = isNameChanged || isEmailChanged || isAvatarChanged;
        }

        private bool IsAvatarSame()
        {
            if (_originalAvatar == null && _avatarImage.sprite == _defaultAvatar)
                return true;

            if (_originalAvatar == null || _avatarImage.sprite == null)
                return false;

            var origPixels = _originalAvatar.texture.GetPixels();
            var currentPixels = _avatarImage.sprite.texture.GetPixels();

            return origPixels.SequenceEqual(currentPixels);
        }

        private void SaveProfile()
        {
            PlayerPrefs.SetString(StringConstants.ACCOUNT_NAME, _usernameInput.text);
            PlayerPrefs.SetString(StringConstants.ACCOUNT_EMAIL, _emailInput.text);
            PlayerPrefs.Save();

            if (_avatarImage.sprite != null && _avatarImage.sprite != _defaultAvatar)
            {
                SpriteSaver.SaveSprite(StringConstants.ACCOUNT_AVATAR_FILENAME, _avatarImage.sprite);
            }

            _originalUsername = _usernameInput.text;
            _originalEmail = _emailInput.text;
            _originalAvatar = _avatarImage.sprite;

            _saveButton.interactable = false;

            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
        }

        private void ChooseAvatar()
        {
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
            WindowManager.ShowWindow<SelectPictureWindow>();
        }

        private void OnBack()
        {
            Hide();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
            WindowManager.ShowWindow<MenuWindow>();
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<AvatarChangedSignal>(OnAvatarChanged);

            _usernameInput.onValueChanged.RemoveAllListeners();
            _emailInput.onValueChanged.RemoveAllListeners();

            _saveButton.onClick.RemoveAllListeners();
            _editUsernameButton.onClick.RemoveAllListeners();
            _editEmailButton.onClick.RemoveAllListeners();
            _editAvatarButton.onClick.RemoveAllListeners();
            _backButton.onClick.RemoveAllListeners();
        }
    }
}
