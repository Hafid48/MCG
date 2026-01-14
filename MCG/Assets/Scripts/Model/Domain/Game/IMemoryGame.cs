using MCG.Model.Domain.Board;
using MCG.Model.Domain.Cards;
using MCG.Services;

namespace MCG.Model.Domain.Game
{
    public delegate void CardEventHandler(ICard card);
    public delegate void CardPairEventHandler(ICard firstCard, ICard secondCard);
    public delegate void GameEventHandler();

    public interface IMemoryGame
    {
        IBoard Board { get; }
        IScoreService ScoreService { get; }

        void SelectCard(ICard card);

        void Reset();

        event CardEventHandler OnCardRevealed;

        event CardPairEventHandler OnCardMatched;

        event CardPairEventHandler OnCardMismatched;

        event GameEventHandler OnGameStarted;

        event GameEventHandler OnGameReset;

        event GameEventHandler OnGameEnded;
    }
}