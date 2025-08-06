using System.Collections.Generic;
using System.Linq;
using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;

public class ConfigDataLoader : IService, IDisposable
{
    private List<ILoader> _loaders;
    private EventBus _eventBus;

    private int _loadedSystems = 0;
    public void Init(List<ILoader> loaders)
    {
        _loaders = loaders;

        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<DataLoadedSignal>(OnConfigLoaded);

        if (_loaders.Any(x => !x.IsLoadingInstant()))
        {

        }

        LoadAll();
    }

    private void OnConfigLoaded(DataLoadedSignal signal)
    {
        _loadedSystems++;

        _eventBus.Invoke(new LoadProgressChangedSignal(((float)_loadedSystems / _loaders.Count)));
        if (_loadedSystems == _loaders.Count)
        {
            _eventBus.Invoke(new AllDataLoadedSignal());
        }
    }

    private void LoadAll()
    {
        foreach (var loader in _loaders)
        {
            loader.Load();
        }
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<DataLoadedSignal>(OnConfigLoaded);
    }
}