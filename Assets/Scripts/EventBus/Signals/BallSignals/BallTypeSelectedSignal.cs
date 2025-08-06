namespace CustomEventBus.Signals
{
    public class BallTypeSelectedSignal
    {
        public readonly BallType BallType;
        public BallTypeSelectedSignal(BallType ballType)
        {
            BallType = ballType;
        }
    }
}