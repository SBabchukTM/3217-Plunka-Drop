namespace CustomEventBus.Signals
{
    public class DisposeBallSignal
    {
        public readonly Ball Ball;

        public DisposeBallSignal(Ball ball)
        {
            Ball = ball;
        }
    }
}