using CustomEventBus.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBall : Ball
{
    protected override void Interact(MultiplierBlock multiplierBlock)
    {
        var resultValue = (int)(ballValue * multiplierBlock.MultiplierValue);

        _eventBus.Invoke(new AddScoreSignal(resultValue));
    }
}
