using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections.Generic;
using TMPro;
using UI.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Image _playerAvatar;

        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _dropButton;

        [SerializeField] private List<Image> _stars;
        [SerializeField] private Sprite _filledStarSprite;
        [SerializeField] private Sprite _emptyStarSprite;

        private EventBus _eventBus;
        private BallType _selectedBallType;

        public void Init()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _eventBus.Subscribe<SetLevelSignal>(RedrawLevel);
            _eventBus.Subscribe<ScoreChangedSignal>(RedrawScore);
            _eventBus.Subscribe<StarsChangedSignal>(RedrawStars);
            _eventBus.Subscribe<BallTypeSelectedSignal>(SetSelectedBall);

            _dropButton.onClick.AddListener(DropBall);
            _pauseButton.onClick.AddListener(OnPause);

            _playerAvatar.sprite = UserProfile.Utils.SpriteSaver.LoadSprite(StringConstants.ACCOUNT_AVATAR_FILENAME) ?? _playerAvatar.sprite;
        }

        public void SetSelectedBall(BallTypeSelectedSignal signal)
        {
            _selectedBallType = signal.BallType;
        }
        private void RedrawLevel(SetLevelSignal signal)
        {
            _levelText.text = "LEVEL " + (signal.LevelData.LevelId).ToString();

            _scoreText.text = "SCORE 0";

            foreach (var star in _stars)
            {
                star.sprite = _emptyStarSprite;
            }
        }

        private void RedrawScore(ScoreChangedSignal signal)
        {
            _scoreText.text = "SCORE " + signal.Score;
        }

        private void RedrawStars(StarsChangedSignal signal)
        {
            for (int i = 0; i < _stars.Count; i++)
            {
                if (i < signal.Stars)
                {
                    _stars[i].sprite = _filledStarSprite;
                }
                else
                {
                    _stars[i].sprite = _emptyStarSprite;
                }
            }
        }


        private void DropBall()
        {
            _eventBus.Invoke(new DropBallSignal(_selectedBallType));
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
        }

        private void OnPause()
        {
            _eventBus.Invoke(new LevelPausedSignal());

            var score = ServiceLocator.Current.Get<ScoreController>().Score;
            PausedWindow pausedWindow = WindowManager.ShowWindow<PausedWindow>();
            pausedWindow.Init(score);
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
        }

        private void OnDestroy()
        {
            _dropButton.onClick.RemoveListener(DropBall);
            _pauseButton.onClick.RemoveListener(OnPause);
        }
    }
}
