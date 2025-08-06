using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class SelectLevelWindow : Window
    {
        [SerializeField] private Button _goBackButton;

        [SerializeField] private FlexibleGridLayoutGroup _levelsGrid;
        [SerializeField] private SelectLevelSlot _levelSlotPrefab;

        private ILevelLoader _levelLoader;

        private void Start()
        {
            _goBackButton.onClick.AddListener(OnGoBackButtonClick);

            _levelLoader = ServiceLocator.Current.Get<ILevelLoader>();
            var levels = _levelLoader.GetLevels();
            levels = levels.OrderBy(x => x.LevelId);
            GenerateLevels(levels);
            Debug.Log(levels);
        }

        private void GenerateLevels(IEnumerable<LevelData> levels)
        {
            foreach (var level in levels)
            {
                var go = GameObject.Instantiate(_levelSlotPrefab, _levelsGrid.transform);
                go.Init(level);
            }
        }

        private void OnGoBackButtonClick()
        {
            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
            WindowManager.ShowWindow<MenuWindow>();
            Hide();
        }
    }
}