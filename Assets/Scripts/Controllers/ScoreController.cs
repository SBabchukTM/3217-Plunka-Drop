using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;

public class ScoreController : IService, IDisposable
{
    private EventBus _eventBus;

    private int _score;
    public int Score => _score;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<StartGameSingal>(OnGameStarted);
        _eventBus.Subscribe<AddScoreSignal>(OnScoreAdded);
        _eventBus.Subscribe<LevelFinishedSignal>(OnLevelFinished);
    }

    private void OnGameStarted(StartGameSingal signal)
    {
        _score = 0;
    }

    private void OnScoreAdded(AddScoreSignal signal)
    {
        _score += signal.Value;
        _eventBus.Invoke(new ScoreChangedSignal(_score));
    }

    private void OnLevelFinished(LevelFinishedSignal signal)
    {
        var level = signal.LevelData;
        var maxScore = GetMaxScore(level.LevelId);
        if (_score > maxScore)
        {
            PlayerPrefs.SetInt(StringConstants.MAX_SCORE, _score);
        }
    }
    public int GetMaxScore(int levelID)
    {
        return PlayerPrefs.GetInt(StringConstants.MAX_SCORE, 0);
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<StartGameSingal>(OnGameStarted);
        _eventBus.Unsubscribe<AddScoreSignal>(OnScoreAdded);
        _eventBus.Unsubscribe<LevelFinishedSignal>(OnLevelFinished);
    }
}