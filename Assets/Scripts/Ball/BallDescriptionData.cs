using System;
using UnityEngine;

[Serializable]
public class BallDescriptionData
{
    [SerializeField] private BallType _ballType;

    [SerializeField] private Sprite _ballSprite;

    [SerializeField] private string _ballDescription;

    public BallType BallType => _ballType;
    public Sprite BallSprite => _ballSprite;
    public string BallDescription => _ballDescription;

}
