namespace CustomEventBus.Signals
{
    public class BallCountChangedSignal
    {
        public readonly BallType BallType;
        public readonly int Count;

        public BallCountChangedSignal(BallType ballType, int count)
        {
            BallType = ballType;
            Count = count;
        }
    }
}