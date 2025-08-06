namespace CustomEventBus.Signals
{
    public class LevelFinishedSignal
    {
        public readonly LevelData LevelData;

        public LevelFinishedSignal(LevelData levelData)
        {
            LevelData = levelData;
        }
    }
}