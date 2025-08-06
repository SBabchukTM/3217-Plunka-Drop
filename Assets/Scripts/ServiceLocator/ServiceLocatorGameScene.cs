using UnityEngine;
using CustomEventBus;
using System.Collections.Generic;
using UI;

public class ServiceLocatorLoaderGameScene : MonoBehaviour
{

    [SerializeField] private SOLevelLoader _SOLevelLoader;
    [SerializeField] private SODescriptionBallLoader _SODescriptionBallLoader;

    [SerializeField] private BallSpawner _ballSpawner;
    [SerializeField] private SoundController _soundController;

    [SerializeField] private GUIHolder _GUIHolder;
    [SerializeField] private HUD _HUD;

    private EventBus _eventBus;
    private ConfigDataLoader _configDataLoader;

    private CountBallController _countBallController;
    private ScoreController _scoreController;
    private StarController _starController;
    private GameController _gameController;
    private Physics2DPauseController _physics2DPausedController; 

    private LevelController _levelController;

    private ILevelLoader _levelLoader;
    private IDescriptionBallLoader _descriptionBallLoader;

    private List<IDisposable> _disposables = new List<IDisposable>();

    private void Awake()
    {
        _eventBus = new EventBus();
        _gameController = new GameController();
        _levelController = new LevelController();
        _countBallController = new CountBallController();
        _scoreController = new ScoreController();
        _starController = new StarController();
        _physics2DPausedController = new Physics2DPauseController();

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
        ServiceLocator.Current.Register(_countBallController);
        ServiceLocator.Current.Register(_scoreController);
        ServiceLocator.Current.Register(_starController);
        ServiceLocator.Current.Register(_gameController);
        ServiceLocator.Current.Register(_levelController);
        ServiceLocator.Current.Register(_physics2DPausedController);

        ServiceLocator.Current.Register<GUIHolder>(_GUIHolder);
        ServiceLocator.Current.Register<SoundController>(_soundController);
        ServiceLocator.Current.Register<BallSpawner>(_ballSpawner);

        ServiceLocator.Current.Register<ILevelLoader>(_levelLoader);
        ServiceLocator.Current.Register<IDescriptionBallLoader>(_descriptionBallLoader);

    }

    private void Initialize()
    {
        _ballSpawner.Init();
        _countBallController.Init();
        _scoreController.Init();
        _starController.Init();
        _soundController.Init();
        _gameController.Init();
        _levelController.Init();
        _physics2DPausedController.Init();

        _HUD.Init();

        var loaders = new List<ILoader>();
        loaders.Add(_levelLoader);
        loaders.Add(_descriptionBallLoader);
        _configDataLoader.Init(loaders);
    }

    private void AddDisposables()
    {
        _disposables.Add(_configDataLoader);
        _disposables.Add(_countBallController);
        _disposables.Add(_scoreController);
        _disposables.Add(_starController);
        _disposables.Add(_gameController);
        _disposables.Add(_levelController);
        _disposables.Add(_physics2DPausedController);

    }

    private void OnDestroy()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }
}
