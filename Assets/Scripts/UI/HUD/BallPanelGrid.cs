using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections.Generic;
using UnityEngine;

public class BallPanelGrid : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private BallTypeSlot _slotPrefab;

    private List<BallTypeSlot> _panels = new();
    private EventBus _eventBus;
    private BallType _currentSelected;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<SetLevelSignal>(SetLevel);
        _eventBus.Subscribe<BallTypeSelectedSignal>(OnTypeSelected);
        _eventBus.Subscribe<BallCountChangedSignal>(OnBallCountChanged);
    }

    private void SetLevel(SetLevelSignal signal)
    {
        var levelData = signal.LevelData;
        _panels.Clear();
        _currentSelected = BallType.None;

        foreach (Transform child in _container)
        {
            Destroy(child.gameObject);
        }

        BallType? firstAvailable = null;

        foreach (var ballData in levelData.BallDatas)
        {
            var panel = Instantiate(_slotPrefab, _container);
            panel.Init(ballData.BallType, ballData.BallsCount, GetBallIcon(ballData.BallType));
            _panels.Add(panel);

            if (firstAvailable == null && ballData.BallsCount > 0)
                firstAvailable = ballData.BallType;
        }

        if (firstAvailable.HasValue)
        {
            _eventBus.Invoke(new BallTypeSelectedSignal(firstAvailable.Value));
        }
    }

    private void OnTypeSelected(BallTypeSelectedSignal signal)
    {
        _currentSelected = signal.BallType;

        foreach (var panel in _panels)
        {
            panel.Highlight(panel.BallType == _currentSelected);
        }
    }

    private void OnBallCountChanged(BallCountChangedSignal signal)
    {
        var panel = _panels.Find(p => p.BallType == signal.BallType);
        if (panel != null)
        {
            panel.UpdateCount(signal.Count);
        }

        if (signal.BallType == _currentSelected && signal.Count <= 0)
        {
            SelectNextAvailable(signal.BallType);
        }
    }

    private void SelectNextAvailable(BallType excluded)
    {
        foreach (var panel in _panels)
        {
            if (panel.BallType != excluded && panel.IsAvailable)
            {
                _eventBus.Invoke(new BallTypeSelectedSignal(panel.BallType));
                return;
            }
        }

        Debug.Log("No available ball types left.");
    }

    private Sprite GetBallIcon(BallType type)
    {
        var descriptionBallLoader = ServiceLocator.Current.Get<IDescriptionBallLoader>();
        var ballsData = descriptionBallLoader.GetBallDescriptionData();

        foreach (var ballData in ballsData)
        {
            if (type == ballData.BallType)
            {
                return ballData.BallSprite;
            }
        }

        return null;
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<SetLevelSignal>(SetLevel);
        _eventBus.Unsubscribe<BallTypeSelectedSignal>(OnTypeSelected);
        _eventBus.Unsubscribe<BallCountChangedSignal>(OnBallCountChanged);
    }
}
