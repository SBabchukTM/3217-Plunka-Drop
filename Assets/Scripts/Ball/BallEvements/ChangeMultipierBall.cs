using CustomEventBus.Signals;
using UnityEngine;

public class ChangeMultiplierBall : Ball
{
    [SerializeField] private float _changedValue = 4f;
    protected override void Interact(MultiplierBlock multiplierBlock)
    {
        var resultValue = (int)(ballValue * multiplierBlock.MultiplierValue);

        _eventBus.Invoke(new AddScoreSignal(resultValue));

        multiplierBlock.SetMultiplier(_changedValue);
    }
}
