using MCG.Model.Data;
using MCG.Model.Domain.Board;
using MCG.Model.Domain.Cards;
using MCG.Services;
using System.Collections.Generic;

namespace MCG.Model.Domain.Game
{
    public delegate void CardEventHandler(ICard card);
    public delegate void CardPairEventHandler(ICard firstCard, ICard secondCard);
    public delegate void GameEventHandler();

    public interface IMemoryGame
    {
        IBoard Board { get; }
        IScoreService ScoreService { get; }
        ISaveLoadService SaveLoadService { get; }

        void SelectCard(ICard card, bool forceSelect = false);

        void Reset(int newRows, int newCols, IReadOnlyList<CardDefinition> newCardDefinitions);

        void Reset(int newRows, int newCols, IReadOnlyList<CardDefinition> newCardDefinitions, IReadOnlyList<CardState> newCardStates);

        void SaveGame();

        void LoadGame();

        event CardEventHandler OnCardRevealed;

        event CardPairEventHandler OnCardMatched;

        event CardPairEventHandler OnCardMismatched;

        event GameEventHandler OnGameStarted;

        event GameEventHandler OnGameReset;

        event GameEventHandler OnGameEnded;
    }
}