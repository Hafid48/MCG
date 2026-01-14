namespace MCG.Services
{
    public delegate void ScoreChangedEventHandler(int oldScore, int newScore);

    public interface IScoreService
    {
        int Score { get; }
        event ScoreChangedEventHandler OnScoreChanged;

        void AddPoints(int points);

        void SetPoints(int points);

        void Reset();
    }
}