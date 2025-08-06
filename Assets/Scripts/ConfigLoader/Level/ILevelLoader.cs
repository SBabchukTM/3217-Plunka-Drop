using System.Collections.Generic;

public interface ILevelLoader : IService, ILoader
{
    public IEnumerable<LevelData> GetLevels();
}
