namespace CustomEventBus.Signals
{
    public class DataLoadedSignal
    {
        public ILoader Loader;
        public DataLoadedSignal(ILoader loader)
        {
            Loader = loader;
        }
    }
}