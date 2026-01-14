namespace MCG.Services
{
    public class ScoreService : IScoreService
    {
        public int Score { get; private set; }

        public event ScoreChangedEventHandler OnScoreChanged;

        public void AddPoints(int points)
        {
            SetPoints(Score + points);
        }

        public void SetPoints(int points)
        {
            int oldScore = Score;
            Score = points;
            OnScoreChanged?.Invoke(oldScore, Score);
        }

        public void ResetScore()
        {
            int oldScore = Score;
            Score = 0;
            OnScoreChanged?.Invoke(oldScore, Score);
        }
    }
}