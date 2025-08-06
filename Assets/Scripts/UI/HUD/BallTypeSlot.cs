using CustomEventBus;
using CustomEventBus.Signals;
using TMPro;
using UI;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;

public class BallTypeSlot : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _ballDescriptionButton;
    [SerializeField] private Image _selectionFrame;

    private BallType _ballType;
    public BallType BallType => _ballType;

    private EventBus _eventBus;

    private bool _isAvailable = true;
    public bool IsAvailable => _isAvailable;

    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _selectButton.onClick.AddListener(OnClicked);
        _ballDescriptionButton.onClick.AddListener(OnDescriptionClicked);
    }


    public void Init(BallType type, int count, Sprite icon)
    {
        _ballType = type;
        _countText.text = count.ToString();
        _icon.sprite = icon;
        _selectionFrame.enabled = false;

        UpdateInteractable(count);
    }

    private void OnClicked()
    {
        if (!_isAvailable) return;

        _eventBus.Invoke(new BallTypeSelectedSignal(_ballType));
    }

    public void UpdateCount(int newCount)
    {
        _countText.text = newCount.ToString();
        UpdateInteractable(newCount);
    }

    private void UpdateInteractable(int count)
    {
        _isAvailable = count > 0;
        _selectButton.interactable = _isAvailable;

        if (!_isAvailable)
        {
            Highlight(false);
        }
    }

    public void Highlight(bool isSelected)
    {
        _selectionFrame.enabled = isSelected;
    }

    private void OnDescriptionClicked()
    {
        var descriptionLoader = ServiceLocator.Current.Get<IDescriptionBallLoader>();
        foreach (var ballData in descriptionLoader.GetBallDescriptionData())
        {
            if (ballData.BallType == _ballType)
            {
                var window = WindowManager.ShowWindow<BallDescriptionWindow>();
                window.Init(ballData);
                return;
            }
        }
    }


    private void OnDestroy()
    {
        _selectButton.onClick.RemoveListener(OnClicked);
        _ballDescriptionButton.onClick.RemoveListener(OnDescriptionClicked);
    }

}
