using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;

public abstract class Ball : MonoBehaviour
{
    private const string MULTIPLIER_TAG = "Multiplier";

    [SerializeField] protected int ballValue = 100;

    protected EventBus _eventBus;

    protected abstract void Interact(MultiplierBlock multiplierBlock);

    protected void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(MULTIPLIER_TAG))
        {
            MultiplierBlock multiplierBlock = collision.GetComponent<MultiplierBlock>();
            if (multiplierBlock != null)
            {
                Interact(multiplierBlock);
                Dispose();

                var soundController = ServiceLocator.Current.Get<SoundController>();
                soundController.PlayMultiplierSound();

                TryVibrate();
            }
        }
    }

    private void Dispose()
    {
        _eventBus.Invoke(new DisposeBallSignal(this));
    }

    private void TryVibrate()
    {
        bool isVibrationEnabled = PlayerPrefs.GetInt(StringConstants.VIBRATION_KEY, 1) == 1;

#if UNITY_ANDROID || UNITY_IOS
        if (isVibrationEnabled)
        {
            Handheld.Vibrate();
        }
#endif
    }
}
