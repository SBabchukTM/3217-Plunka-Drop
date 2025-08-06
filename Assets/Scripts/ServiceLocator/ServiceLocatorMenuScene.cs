using UnityEngine;
using CustomEventBus;
using System.Collections.Generic;
using UI;
using UI.Windows;

public class ServiceLocatorLoaderMenuScene : MonoBehaviour
{

    [SerializeField] private SOLevelLoader _SOLevelLoader;
    [SerializeField] private SODescriptionBallLoader _SODescriptionBallLoader;

    [SerializeField] private FakePlayerGenerator _fakePlayerGenerator;
    [SerializeField] private SoundController _soundController;
    [SerializeField] private GUIHolder _GUIHolder;

    private EventBus _eventBus;
    private ConfigDataLoader _configDataLoader;

    private ScoreController _scoreController;
    private StarController _starController;


    private ILevelLoader _levelLoader;
    private IDescriptionBallLoader _descriptionBallLoader;

    private List<IDisposable> _disposables = new List<IDisposable>();

    private void Awake()
    {
        _eventBus = new EventBus();
        _scoreController = new ScoreController();
        _starController = new StarController();

        _configDataLoader = new ConfigDataLoader();

        _levelLoader = _SOLevelLoader;
        _descriptionBallLoader = _SODescriptionBallLoader;

        RegisterServices();
        Initialize();
        AddDisposables();
    }

    private void RegisterServices()
    {
        ServiceLocator.Init();

        ServiceLocator.Current.Register(_eventBus);
        ServiceLocator.Current.Register(_scoreController);
        ServiceLocator.Current.Register(_starController);

        ServiceLocator.Current.Register<FakePlayerGenerator>(_fakePlayerGenerator);
        ServiceLocator.Current.Register<SoundController>(_soundController);
        ServiceLocator.Current.Register<GUIHolder>(_GUIHolder);

        ServiceLocator.Current.Register<ILevelLoader>(_levelLoader);
        ServiceLocator.Current.Register<IDescriptionBallLoader>(_descriptionBallLoader);

    }

    private void Initialize()
    {
        _scoreController.Init();
        _starController.Init();
        _soundController.Init();

        var loaders = new List<ILoader>();
        loaders.Add(_levelLoader);
        loaders.Add(_descriptionBallLoader);
        _configDataLoader.Init(loaders);

        _soundController.PlayMenuMusic();
    }

    private void AddDisposables()
    {
        _disposables.Add(_configDataLoader);
        _disposables.Add(_scoreController);
        _disposables.Add(_starController);
    }

    [ContextMenu("Delete All PlayerPrefs")]
    public void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    private void OnDestroy()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }
}
