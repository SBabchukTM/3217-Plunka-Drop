using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BallDescriptionConfig", menuName = "ScriptableObjects/BallDescriptionConfig", order = 2)]
public class BallDescriptionConfig : ScriptableObject
{
    [SerializeField] public List<BallDescriptionData> _ballDescriptionConfig;

    public List<BallDescriptionData> BallDescriptionData => _ballDescriptionConfig;
}