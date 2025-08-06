using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Image _avatarImage;
    [SerializeField] private Sprite _defaultAvatar;

    public void Init(string playerName, int score, Sprite avatar = null)
    {
        _nameText.text = playerName;
        _scoreText.text = score.ToString();

        if (avatar != null)
            _avatarImage.sprite = avatar;
        else
            _avatarImage.sprite = _defaultAvatar;
    }
}
