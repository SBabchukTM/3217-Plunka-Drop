using CustomEventBus.Signals;
using CustomEventBus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SODescriptionBallLoader : MonoBehaviour, IDescriptionBallLoader
{
    [SerializeField] private BallDescriptionConfig _config;

    public IEnumerable<BallDescriptionData> GetBallDescriptionData()
    {
        return _config.BallDescriptionData;
    }

    public bool IsLoaded()
    {
        return true;
    }

    public void Load()
    {
        var eventBus = ServiceLocator.Current.Get<EventBus>();
        eventBus.Invoke(new DataLoadedSignal(this));
    }

    public bool IsLoadingInstant()
    {
        return true;
    }
}
