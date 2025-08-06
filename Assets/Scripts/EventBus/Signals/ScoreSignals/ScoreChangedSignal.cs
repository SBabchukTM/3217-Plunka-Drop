namespace CustomEventBus.Signals
{
    public class ScoreChangedSignal
    {
        public readonly int Score;

        public ScoreChangedSignal(int score)
        {
            Score = score;
        }
    }
}