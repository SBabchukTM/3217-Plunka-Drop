using System.Collections.Generic;
using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallSpawner : MonoBehaviour, IService
{
    [SerializeField] private Transform _parent;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _defaultY;

    public float MinX => _minX;
    public float MaxX => _maxX;

    private readonly Dictionary<string, ObjectPool<Ball>> _pools =
        new Dictionary<string, ObjectPool<Ball>>();

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<SpawnBallSignal>(Spawn);
        _eventBus.Subscribe<DisposeBallSignal>(Dispose);
        _eventBus.Subscribe<RestartLevelSignal>(DisposeAllPool, -1);
    }

    private void Spawn(SpawnBallSignal signal)
    {
        var ball = signal.BallPrefab;
        var pool = GetPool(ball);

        var item = pool.Get();
        item.transform.parent = _parent;
        item.transform.position = RandomizeSpawnPosition();
    }

    private void Dispose(DisposeBallSignal signal)
    {
        var interactable = signal.Ball;
        var pool = GetPool(interactable);
        pool.Release(interactable);
    }

    private void DisposeAllPool(RestartLevelSignal signal)
    {
        foreach (var pool in _pools.Values)
        {
            pool.ReleaseAllActive();
        }
    }

    private ObjectPool<Ball> GetPool(Ball ball)
    {
        var objectTypeStr = ball.GetType().ToString();
        ObjectPool<Ball> pool;

        if (!_pools.ContainsKey(objectTypeStr))
        {
            pool = new ObjectPool<Ball>(ball, 5);
            _pools.Add(objectTypeStr, pool);
        }
        else
        {
            pool = _pools[objectTypeStr];
        }

        return pool;
    }

    private Vector3 RandomizeSpawnPosition()
    {
        float x = Random.Range(_minX, _maxX);
        return new Vector3(x, _defaultY, 0);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<SpawnBallSignal>(Spawn);
        _eventBus.Unsubscribe<DisposeBallSignal>(Dispose);
    }
}