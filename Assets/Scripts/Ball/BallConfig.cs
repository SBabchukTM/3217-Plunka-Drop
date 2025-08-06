using System.Collections.Generic;
using UnityEngine;

public class BallConfig : ScriptableObject
{
    [SerializeField] public List<BallData> _ballConfig;

    public List<BallData> BallData => _ballConfig;
}