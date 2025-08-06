using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;

public class StarController : IService, IDisposable
{

    private int _stars;
    public int Stars => _stars;

    private EventBus _eventBus;

    private LevelData _levelData;

    private int _currentScore;
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<SetLevelSignal>(SetLevel);
        _eventBus.Subscribe<ScoreChangedSignal>(OnScoreChanged, -1);
        _eventBus.Subscribe<LevelFinishedSignal>(OnLevelFinished, -1);
    }

    private void SetLevel(SetLevelSignal signal)
    {
        _levelData = signal.LevelData;
    }

    private void OnScoreChanged(ScoreChangedSignal signal)
    {
        _currentScore = ServiceLocator.Current.Get<ScoreController>().Score;

        int stars = 0;

        if (_currentScore >= _levelData.MinScoreForWin)
            stars += 1;
            
        if (_currentScore >= _levelData.ScoreForTwoStars)
            stars += 1;

        if (_currentScore >= _levelData.ScoreForThreeStars)
            stars += 1;

        _stars = stars;

        _eventBus.Invoke(new StarsChangedSignal(_stars));
    }

    private void OnLevelFinished(LevelFinishedSignal signal)
    { 
        var maxStars = GetMaxStars(_levelData.LevelId);
        if (_stars > maxStars)
        {
            PlayerPrefs.SetInt(StringConstants.MAX_LEVEL_STARS + _levelData.LevelId, _stars);
        }
    }

    public int GetMaxStars(int levelId)
    {
        return PlayerPrefs.GetInt(StringConstants.MAX_LEVEL_STARS + levelId, 0);
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<SetLevelSignal>(SetLevel);
        _eventBus.Unsubscribe<ScoreChangedSignal>(OnScoreChanged);
        _eventBus.Unsubscribe<LevelFinishedSignal>(OnLevelFinished);
    }
}
