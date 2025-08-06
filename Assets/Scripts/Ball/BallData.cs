using System;
using UnityEngine;

[Serializable]
public class BallData
{
    [SerializeField] private BallType _ballType;

    [SerializeField] private Ball _ballPrefab;

    [SerializeField] private int _ballsCount;

    
    public BallType BallType => _ballType;
    public Ball Prefab => _ballPrefab;
    public int BallsCount => _ballsCount;
    
}
