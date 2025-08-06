using CustomEventBus.Signals;
using UnityEngine;

public class MultiplierSensitiveBall : Ball
{
    protected override void Interact(MultiplierBlock multiplierBlock)
    {
        var resultValue = (int)(ballValue * multiplierBlock.MultiplierValue);
        _eventBus.Invoke(new AddScoreSignal(resultValue));

        if (multiplierBlock.MultiplierValue <= 2f)
        {
            _eventBus.Invoke(new SpawnBallSignal(this));
            _eventBus.Invoke(new ActivatedMultiplierSensitiveBallSignal());
        }
    }
}
