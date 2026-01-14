using UnityEngine;
using TMPro;
using MCG.Services;

namespace MCG.View
{
    public class ScoreView : MonoBehaviour
    {
        private TextMeshProUGUI _scoreText;
        private IScoreService _scoreService;

        private void Awake()
        {
            _scoreText = GetComponent<TextMeshProUGUI>();
        }

        public void Initialize(IScoreService scoreService)
        {
            _scoreService = scoreService;
            _scoreService.OnScoreChanged += OnScoreChanged;
        }

        private void OnScoreChanged(int oldScore, int newScore)
        {
            SetScore(newScore);
        }

        public void SetScore(int score)
        {
            _scoreText.text = $"Matches : {score}";
        }
    }
}