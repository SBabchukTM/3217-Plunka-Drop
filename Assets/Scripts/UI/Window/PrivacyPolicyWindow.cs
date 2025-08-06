using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class PrivacyPolicyWindow : Window
    {
        [SerializeField] private Button _goBackButton;


        private void Start()
        {
            _goBackButton.onClick.AddListener(OnGoBackButtonClick);
        }

        private void OnGoBackButtonClick()
        {
            Hide();
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
            WindowManager.ShowWindow<MenuWindow>();
        }

        private void OnDestroy()
        {
            _goBackButton.onClick.RemoveAllListeners();
        }
    }
}
