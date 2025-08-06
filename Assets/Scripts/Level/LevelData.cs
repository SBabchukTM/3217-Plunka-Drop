using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    [SerializeField] private int _levelId;

    [SerializeField] private int _minScoreForWin;

    [SerializeField] private int _scoreForTwoStars;

    [SerializeField] private int _scoreForThreeStars;

    [SerializeField] private List<BallData> _ballData;

    public int LevelId => _levelId;
    public int MinScoreForWin => _minScoreForWin;
    public int ScoreForTwoStars => _scoreForTwoStars;
    public int ScoreForThreeStars => _scoreForThreeStars;
    public List<BallData> BallDatas => _ballData;
}
