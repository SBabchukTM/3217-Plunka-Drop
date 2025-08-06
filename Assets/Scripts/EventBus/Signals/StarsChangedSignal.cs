namespace CustomEventBus.Signals
{
    public class StarsChangedSignal
    {
        public readonly int Stars;

        public StarsChangedSignal(int stars)
        {
            Stars = stars;
        }
    }
}