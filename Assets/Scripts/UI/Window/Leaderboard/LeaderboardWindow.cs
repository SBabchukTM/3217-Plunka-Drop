using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class LeaderboardWindow : Window
    {
        [SerializeField] private Button _goBackButton;

        [SerializeField] private LeaderboardSlot _leaderboardSlotPrefab;
        [SerializeField] private RectTransform _listContainer;

        private void Start()
        {
            _goBackButton.onClick.AddListener(OnGoBackButtonClick);

            FillLeaderboard();
        }

        private void FillLeaderboard()
        {
            var fakePlayers = ServiceLocator.Current.Get<FakePlayerGenerator>().GetFakePlayers();

            string playerName = PlayerPrefs.GetString(StringConstants.ACCOUNT_NAME, "Username");
            int playerScore = PlayerPrefs.GetInt(StringConstants.MAX_SCORE, 0);
            var avatar = UserProfile.Utils.SpriteSaver.LoadSprite(StringConstants.ACCOUNT_AVATAR_FILENAME);

            FakePlayer realPlayer = new FakePlayer(-1, playerName, playerScore);

            List<FakePlayer> allPlayers = new List<FakePlayer>(fakePlayers);
            allPlayers.Add(realPlayer);

            allPlayers.Sort((a, b) => b.Score.CompareTo(a.Score));

            foreach (var player in allPlayers)
            {
                var slot = Instantiate(_leaderboardSlotPrefab, _listContainer);

                Sprite playerAvatar = player.Id == -1 ? avatar : null;

                slot.Init(player.Name, player.Score, playerAvatar);
            }
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
