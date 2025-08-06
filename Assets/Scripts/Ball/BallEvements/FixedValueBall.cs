using CustomEventBus.Signals;
using UnityEngine;

public class FixedValueBall : Ball
{
    [SerializeField] private int _fixedValue = 500;
    protected override void Interact(MultiplierBlock multiplierBlock)
    {
        _eventBus.Invoke(new AddScoreSignal(ballValue + _fixedValue));
    }
}
