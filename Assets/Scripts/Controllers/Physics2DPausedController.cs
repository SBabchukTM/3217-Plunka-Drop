using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections.Generic;
using UnityEngine;

public class Physics2DPauseController : IService, IDisposable
{
    private Dictionary<Rigidbody2D, (Vector2 velocity, float angularVelocity)> _savedVelocities = new();

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<LevelPausedSignal>(FreezeAll);
        _eventBus.Subscribe<LevelUnpausedSignal>(UnfreezeAll);
    }

    private void FreezeAll(LevelPausedSignal signal)
    {
        _savedVelocities.Clear();

        foreach (var rb in GameObject.FindObjectsOfType<Rigidbody2D>())
        {
            if (rb.bodyType != RigidbodyType2D.Dynamic) continue;

            _savedVelocities[rb] = (rb.velocity, rb.angularVelocity);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void UnfreezeAll(LevelUnpausedSignal signal)
    {
        foreach (var rb in GameObject.FindObjectsOfType<Rigidbody2D>())
        {
            if (rb.bodyType != RigidbodyType2D.Dynamic) continue;

            rb.constraints = RigidbodyConstraints2D.None;

            if (_savedVelocities.TryGetValue(rb, out var saved))
            {
                rb.velocity = saved.velocity;
                rb.angularVelocity = saved.angularVelocity;
            }
        }

        _savedVelocities.Clear();
    }


    public void Dispose()
    {
        _eventBus.Unsubscribe<LevelPausedSignal>(FreezeAll);
        _eventBus.Unsubscribe<LevelUnpausedSignal>(UnfreezeAll);
    }
}
