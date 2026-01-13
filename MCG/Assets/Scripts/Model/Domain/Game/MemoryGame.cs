using MCG.Model.Domain.Board;

namespace MCG.Model.Domain.Game
{
    public class MemoryGame : IMemoryGame
    {
        public IBoard Board { get; private set; }
        public event CardEventHandler OnCardRevealed;
        public event CardPairEventHandler OnCardMatched;
        public event CardPairEventHandler OnCardMismatched;
        public event GameEventHandler OnGameStarted;
        public event GameEventHandler OnGameReset;
        public event GameEventHandler OnGameEnded;

        public MemoryGame(IBoard board)
        {
            Board = board;
        }

        public void SelectCard(ICard card)
        {
            
        }

        public void Reset()
        {
            
        }
    }
}