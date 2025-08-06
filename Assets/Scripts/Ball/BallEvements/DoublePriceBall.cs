using CustomEventBus.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePriceBall : Ball
{
    protected override void Interact(MultiplierBlock multiplierBlock)
    {
        var resultValue = (int)(2 * (ballValue * multiplierBlock.MultiplierValue));

        _eventBus.Invoke(new AddScoreSignal(resultValue));
    }
}
