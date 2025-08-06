namespace CustomEventBus.Signals
{
    public class SpawnBallSignal
    {
        public readonly Ball BallPrefab;
        public SpawnBallSignal(Ball ballPrefab)
        {
            BallPrefab = ballPrefab;
        }
    }
}