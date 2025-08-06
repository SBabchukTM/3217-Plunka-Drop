using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDescriptionBallLoader : IService, ILoader
{
    public IEnumerable<BallDescriptionData> GetBallDescriptionData();
}