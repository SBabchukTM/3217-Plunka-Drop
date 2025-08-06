using UnityEngine;
using UnityEngine.UI;
using System.IO;
using CustomEventBus;
using CustomEventBus.Signals;
using UserProfile.Utils;

namespace UI.Windows
{
    public class SelectPictureWindow : Window
    {
        [SerializeField] private Button _selectFromGalleryButton;
        [SerializeField] private Button _takePhotoButton;
        [SerializeField] private Button _removeAvatarButton;
        [SerializeField] private Button _closeButton;

        private EventBus _eventBus;

        private void Start()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();

            _selectFromGalleryButton.onClick.AddListener(SelectPictureFromGallery);
            _takePhotoButton.onClick.AddListener(MakeNewPicture);
            _removeAvatarButton.onClick.AddListener(RemoveAvatar);
            _closeButton.onClick.AddListener(() => Hide());
        }

        private void SelectPictureFromGallery()
        {
            NativeGallery.GetImageFromGallery(LoadImageByPath, "Select Avatar", "image/*");
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();

            _removeAvatarButton.interactable = true;
            Hide();
        }

        private void MakeNewPicture()
        {
            NativeCamera.TakePicture(LoadImageByPath);
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();

            _removeAvatarButton.interactable = true;
            Hide();
        }

        private void LoadImageByPath(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return;

            Texture2D texture = NativeGallery.LoadImageAtPath(path);
            if (texture == null)
                return;

            StartCropping(texture);
        }

        private void StartCropping(Texture2D texture)
        {
            var settings = new ImageCropper.Settings
            {
                markTextureNonReadable = false,
                selectionMinAspectRatio = 1f,
                selectionMaxAspectRatio = 1f
            };

            ImageCropper.Instance.Show(texture, SetIconAfterCrop, settings);
        }

        private void SetIconAfterCrop(bool result, Texture original, Texture cropped)
        {
            if (!result)
                return;

            Texture2D texture = (Texture2D)cropped;

            Sprite croppedSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

            SpriteSaver.SaveSprite(StringConstants.ACCOUNT_AVATAR_FILENAME, croppedSprite);
            PlayerPrefs.SetString(StringConstants.ACCOUNT_AVATAR_PATH, Path.Combine(Application.persistentDataPath, StringConstants.ACCOUNT_AVATAR_FILENAME));

            _eventBus.Invoke(new AvatarChangedSignal(croppedSprite));
        }

        private void RemoveAvatar()
        {
            string avatarPath = PlayerPrefs.GetString(StringConstants.ACCOUNT_AVATAR_PATH, "");
            if (!string.IsNullOrEmpty(avatarPath) && File.Exists(avatarPath))
            {
                File.Delete(avatarPath);
            }

            PlayerPrefs.DeleteKey(StringConstants.ACCOUNT_AVATAR_PATH);

            _eventBus.Invoke(new AvatarChangedSignal(null));

            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();

            Hide();
            _removeAvatarButton.interactable = false;
        }

        private void OnDestroy()
        {
            _selectFromGalleryButton.onClick.RemoveAllListeners();
            _takePhotoButton.onClick.RemoveAllListeners();
            _removeAvatarButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
        }
    }
}
