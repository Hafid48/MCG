using MCG.Model.Domain.Board;
using MCG.Model.Domain.Cards;
using MCG.Services;
using System.Collections.Generic;

namespace MCG.Model.Domain.Game
{
    public class MemoryGame : IMemoryGame
    {
        public IBoard Board { get; private set; }
        public IScoreService ScoreService { get; private set; }
        public event CardEventHandler OnCardRevealed;
        public event CardPairEventHandler OnCardMatched;
        public event CardPairEventHandler OnCardMismatched;
        public event GameEventHandler OnGameStarted;
        public event GameEventHandler OnGameReset;
        public event GameEventHandler OnGameEnded;
        private readonly Queue<ICard> _selectedCardsQueue = new Queue<ICard>();
        private const int MAX_SELECTION = 2;

        public MemoryGame(IBoard board, IScoreService scoreService)
        {
            Board = board;
            ScoreService = scoreService;
        }

        public void SelectCard(ICard card)
        {
            if (card == null || card.State != CardState.Hidden)
                return;
            card.Reveal();
            OnCardRevealed?.Invoke(card);
            _selectedCardsQueue.Enqueue(card);
            if (_selectedCardsQueue.Count == MAX_SELECTION)
                CheckSelection();
        }

        private void CheckSelection()
        {
            ICard firstCard = _selectedCardsQueue.Dequeue();
            ICard secondCard = _selectedCardsQueue.Dequeue();
            if (firstCard.Definition.Id == secondCard.Definition.Id)
            {
                firstCard.Match();
                secondCard.Match();
                OnCardMatched?.Invoke(firstCard, secondCard);
                ScoreService.AddPoints(1);
                if (HasGameEnded())
                    OnGameEnded?.Invoke();
            }
            else
                OnCardMismatched?.Invoke(firstCard, secondCard);
        }

        private bool HasGameEnded()
        {
            foreach (ICard card in Board.Cards)
            {
                if (card.State == CardState.Empty)
                    continue; // skip empty slots
                if (card.State != CardState.Matched)
                    return false;
            }
            return true;
        }

        public void Reset()
        {
            
        }
    }
}