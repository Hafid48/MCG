using MCG.Model.Domain.Board;
using MCG.Model.Domain.Game;
using MCG.View;
using UnityEngine;

namespace MCG.Application
{
    public class Startup : MonoBehaviour
    {
        [SerializeField]
        private BoardView _boardView;
        private IBoard _board;
        private IMemoryGame _memoryGame;

        private void Start()
        {
            _board = new Board(_boardView.Rows, _boardView.Columns);
            _board.Initialize(_boardView.CardDefinitions);
            _memoryGame = new MemoryGame(_board);
            _boardView.Initialize(_memoryGame);
        }
    }
}