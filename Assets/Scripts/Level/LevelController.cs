using System.Linq;
using CustomEventBus;
using CustomEventBus.Signals;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LevelController : IService, IDisposable
{
    private ILevelLoader _levelLoader;
    private int _currentLevelId;
    private LevelData _currentLevelData;

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<LevelPassedSignal>(LevelPassed);
        _eventBus.Subscribe<NextLevelSignal>(NextLevel);
        _eventBus.Subscribe<RestartLevelSignal>(RestartLevel);

        _levelLoader = ServiceLocator.Current.Get<ILevelLoader>();
        _currentLevelId = PlayerPrefs.GetInt(StringConstants.CURRENT_LEVEL, 1);

        OnInit();
    }

    private async void OnInit()
    {
        await UniTask.WaitUntil(_levelLoader.IsLoaded);
        _currentLevelData = _levelLoader.GetLevels().FirstOrDefault(x => x.LevelId == _currentLevelId);
        if (_currentLevelData == null)
        {
            Debug.LogErrorFormat("Can't find level with id {0}", _currentLevelId);
            return;
        }
        _eventBus.Invoke(new SetLevelSignal(_currentLevelData));
    }

    private void NextLevel(NextLevelSignal signal)
    {
        var levels = _levelLoader.GetLevels();
        var maxLevelId = levels.Max(x => x.LevelId);

        if (_currentLevelId >= maxLevelId)
        {
            Debug.Log("No next level. Returning to menu or showing endgame screen.");

            ServiceLocator.Current.Get<SoundController>().PlayButtonSound();
            UnityEngine.SceneManagement.SceneManager.LoadScene(StringConstants.MENU_SCENE);
            return;
        }

        _currentLevelId++;
        PlayerPrefs.SetInt(StringConstants.CURRENT_LEVEL, _currentLevelId);
        SelectLevel(_currentLevelId);
    }


    private void RestartLevel(RestartLevelSignal signal)
    {
        _eventBus.Invoke(new SetLevelSignal(_currentLevelData));
    }

    private void SelectLevel(int level)
    {
        _currentLevelId = level;
        _currentLevelData = _levelLoader.GetLevels().FirstOrDefault(x => x.LevelId == _currentLevelId);
        _eventBus.Invoke(new SetLevelSignal(_currentLevelData));
    }

    private void LevelPassed(LevelPassedSignal signal)
    {
        var score = ServiceLocator.Current.Get<ScoreController>().Score;
        if(score > _currentLevelData.MinScoreForWin)
        {
            _eventBus.Invoke(new LevelFinishedSignal(_currentLevelData));
        }
        else
        {
            _eventBus.Invoke(new LevelLostSignal());
        }
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<LevelPassedSignal>(LevelPassed);
        _eventBus.Unsubscribe<NextLevelSignal>(NextLevel);
        _eventBus.Unsubscribe<RestartLevelSignal>(RestartLevel);
    }
}