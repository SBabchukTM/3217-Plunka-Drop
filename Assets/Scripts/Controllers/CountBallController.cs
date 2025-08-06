using System;
using System.Collections.Generic;
using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;

public class CountBallController : IService, CustomEventBus.IDisposable
{
    private bool _isLevelRunning = false;
    private LevelData _levelData;

    private readonly Dictionary<BallType, int> _ballInventory = new();
    private readonly Dictionary<BallType, Ball> _ballPrefabs = new();

    private EventBus _eventBus;

    private int _totalBallCount; 

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<SetLevelSignal>(LevelSet);
        _eventBus.Subscribe<StartGameSingal>(GameStart);
        _eventBus.Subscribe<StopGameSingal>(GameStop);
        _eventBus.Subscribe<DropBallSignal>(OnDropBallRequested);
        _eventBus.Subscribe<DisposeBallSignal>(OnDisposeBallSignal);
        _eventBus.Subscribe<ActivatedMultiplierSensitiveBallSignal>(ActivatedMultiplierSensitiveBall);
    }

    private void LevelSet(SetLevelSignal signal)
    {
        _levelData = signal.LevelData;

        _ballInventory.Clear();
        _ballPrefabs.Clear();

        _totalBallCount = 0;

        foreach (var ballData in _levelData.BallDatas)
        {
            BallType ballType = ballData.BallType;
            _ballInventory[ballType] = ballData.BallsCount;
            _ballPrefabs[ballType] = ballData.Prefab;

            _totalBallCount += ballData.BallsCount;
        }
    }

    private void GameStart(StartGameSingal signal)
    {
        _isLevelRunning = true;
    }

    private void GameStop(StopGameSingal signal)
    {
        _isLevelRunning = false;
    }

    private void OnDropBallRequested(DropBallSignal signal)
    {
        if (!_isLevelRunning)
            return;

        BallType ballType = signal.BallType;

        if (!_ballInventory.ContainsKey(ballType))
        {
            Debug.LogWarning($"No such ball type: {ballType}");
            return;
        }

        if (_ballInventory[ballType] <= 0)
        {
            Debug.Log("No more balls of this type.");
            return;
        }

        Ball prefab = _ballPrefabs[ballType];
        _eventBus.Invoke(new SpawnBallSignal(prefab));
        _ballInventory[ballType]--;
        _eventBus.Invoke(new BallCountChangedSignal(ballType, _ballInventory[ballType]));
    }

    private void ActivatedMultiplierSensitiveBall(ActivatedMultiplierSensitiveBallSignal signal)
    {
        _totalBallCount++;
    }

    private void OnDisposeBallSignal(DisposeBallSignal signal)
    {
        _totalBallCount--;

        if (_totalBallCount <= 0)
        {
            Debug.Log("Level Completed: all balls used and disposed.");
            _eventBus.Invoke(new LevelPassedSignal());
        }
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<SetLevelSignal>(LevelSet);
        _eventBus.Unsubscribe<StartGameSingal>(GameStart);
        _eventBus.Unsubscribe<StopGameSingal>(GameStop);
        _eventBus.Unsubscribe<DropBallSignal>(OnDropBallRequested);
        _eventBus.Unsubscribe<DisposeBallSignal>(OnDisposeBallSignal);
        _eventBus.Unsubscribe<ActivatedMultiplierSensitiveBallSignal>(ActivatedMultiplierSensitiveBall);
    }
}
