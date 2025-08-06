using CustomEventBus;
using CustomEventBus.Signals;
using UI;
using UI.Windows;
using UnityEngine;

public class GameController : IService, IDisposable
{
    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<LevelFinishedSignal>(LevelFinished);
        _eventBus.Subscribe<LevelLostSignal>(LevelLost);
        _eventBus.Subscribe<LevelPausedSignal>(LevelPaused);
        _eventBus.Subscribe<LevelUnpausedSignal>(LevelUnpaused);
        _eventBus.Subscribe<SetLevelSignal>(StartGame, -1);
    }

    public void StartGame(SetLevelSignal signal)
    {
        _eventBus.Invoke(new StartGameSingal());
        ServiceLocator.Current.Get<SoundController>().PlayGameMusic();
    }

    public void StopGame()
    {
        _eventBus.Invoke(new StopGameSingal());
    }

    private void LevelFinished(LevelFinishedSignal signal)
    {
        var score = ServiceLocator.Current.Get<ScoreController>().Score;
        var countStar = ServiceLocator.Current.Get<StarController>().Stars;
        YouWinWindow youWinWindow = WindowManager.ShowWindow<YouWinWindow>();
        youWinWindow.Init(score, countStar);
        ServiceLocator.Current.Get<SoundController>().PlayWinSound();
        StopGame();
    }

    private void LevelLost(LevelLostSignal signal)
    {
        var score = ServiceLocator.Current.Get<ScoreController>().Score;
        YouLoseWindow youLoseWindow = WindowManager.ShowWindow<YouLoseWindow>();
        youLoseWindow.Init(score);
        ServiceLocator.Current.Get<SoundController>().PlayLoseSound();
        StopGame();
    }

    private void LevelPaused(LevelPausedSignal signal)
    {
        StopGame();
    }

    private void LevelUnpaused(LevelUnpausedSignal signal)
    {
        _eventBus.Invoke(new StartGameSingal());
    }


    public void Dispose()
    {
        _eventBus.Unsubscribe<LevelFinishedSignal>(LevelFinished);
        _eventBus.Unsubscribe<LevelLostSignal>(LevelLost);
        _eventBus.Unsubscribe<LevelPausedSignal>(LevelPaused);
        _eventBus.Unsubscribe<LevelUnpausedSignal>(LevelUnpaused);
        _eventBus.Unsubscribe<SetLevelSignal>(StartGame);
    }
}
