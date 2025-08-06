using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class BallDescriptionWindow : Window
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Button _closeButton;

        public void Init(BallDescriptionData data)
        {
            _icon.sprite = data.BallSprite;
            _descriptionText.text = data.BallDescription;

            _closeButton.onClick.AddListener(OnClose);
        }

        private void OnClose()
        {
            Hide();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
        }
    }
}
