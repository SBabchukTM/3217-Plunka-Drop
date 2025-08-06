namespace CustomEventBus.Signals
{
    public class SetLevelSignal
    {
        public readonly LevelData LevelData;

        public SetLevelSignal(LevelData levelData)
        {
            LevelData = levelData;
        }
    }
}