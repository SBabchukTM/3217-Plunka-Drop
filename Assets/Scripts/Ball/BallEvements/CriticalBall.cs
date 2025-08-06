using CustomEventBus.Signals;
using UnityEngine;

public class CriticalBall : Ball
{
    [SerializeField] private float _criticalChance = 0.1f;
    protected override void Interact(MultiplierBlock multiplierBlock)
    {
        float chance = Random.Range(0f, 1f);
        int resultValue;

        if (chance <= _criticalChance)
        {
            resultValue = (int)(ballValue * multiplierBlock.MultiplierValue * 5);
        }
        else
        {
            resultValue = (int)(ballValue * multiplierBlock.MultiplierValue);
        }

        _eventBus.Invoke(new AddScoreSignal(resultValue));
    }
}
