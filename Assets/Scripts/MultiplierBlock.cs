using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiplierBlock : MonoBehaviour
{
    [SerializeField] private float _multiplierValue;
    [SerializeField] private TextMeshPro _multiplierText;
    public float MultiplierValue => _multiplierValue;

    private float _savedMultiplierValue;

    private EventBus _eventBus;

    private void Start()
    {
        _savedMultiplierValue = _multiplierValue;

        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<SetLevelSignal>(RedrawMultiplier);
    }

    private void RedrawMultiplier(SetLevelSignal signal)
    {
        _multiplierValue = _savedMultiplierValue;
        _multiplierText.text = _savedMultiplierValue.ToString() + "x";
    }

    public void SetMultiplier(float newMultiplier)
    {
        _multiplierValue = newMultiplier;
        _multiplierText.text = newMultiplier.ToString() + "x";
    }

}
