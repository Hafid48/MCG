using MCG.Model.Domain.Board;
using MCG.Model.Domain.Game;
using MCG.Services;
using MCG.View;
using UnityEngine;

namespace MCG.Application
{
    public class Startup : MonoBehaviour
    {
        [SerializeField]
        private BoardView _boardView;
        [SerializeField]
        private ScoreView _scoreView;
        [SerializeField]
        private SaveLoadView _saveLoadView;
        private IBoard _board;
        private IMemoryGame _memoryGame;
        private IScoreService _scoreService;
        private ISaveLoadService _saveLoadService;

        private void Start()
        {
            _scoreService = new ScoreService();
            _saveLoadService = new PlayerPrefsSaveLoadService();
            _board = new Board(_boardView.Rows, _boardView.Columns);
            _board.Initialize(_boardView.CardDefinitions);
            _memoryGame = new MemoryGame(_board, _scoreService, _saveLoadService);
            _boardView.Initialize(_memoryGame);
            _scoreView.Initialize(_scoreService);
            _saveLoadView.Initialize(_memoryGame);
        }
    }
}