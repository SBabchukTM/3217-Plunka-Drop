namespace CustomEventBus.Signals
{
    public class DropBallSignal
    {
        public readonly BallType BallType;
        public DropBallSignal(BallType ballType)
        {
            BallType = ballType;
        }
    }
}