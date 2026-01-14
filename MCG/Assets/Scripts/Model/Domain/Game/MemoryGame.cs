using MCG.Model.Data;
using MCG.Model.Domain.Board;
using MCG.Model.Domain.Cards;
using MCG.Services;
using MCG.View;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MCG.Model.Domain.Game
{
    public class MemoryGame : IMemoryGame
    {
        public IBoard Board { get; private set; }
        public IScoreService ScoreService { get; private set; }
        public ISaveLoadService SaveLoadService { get; private set; }
        public event CardEventHandler OnCardRevealed;
        public event CardPairEventHandler OnCardMatched;
        public event CardPairEventHandler OnCardMismatched;
        public event GameEventHandler OnGameStarted;
        public event GameEventHandler OnGameReset;
        public event GameEventHandler OnGameEnded;
        private readonly Queue<ICard> _selectedCardsQueue = new Queue<ICard>();
        private const int MAX_SELECTION = 2;

        public MemoryGame(IBoard board, IScoreService scoreService, ISaveLoadService saveLoadService)
        {
            Board = board;
            ScoreService = scoreService;
            SaveLoadService = saveLoadService;
            OnGameStarted?.Invoke();
        }

        public void SelectCard(ICard card, bool forceSelect)
        {
            if (card == null || (!forceSelect && card.State != CardState.Hidden))
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

        public void Reset(int newRows, int newColumns, IReadOnlyList<CardDefinition> newCardDefinitions)
        {
            Board = new Board.Board(newRows, newColumns);
            Board.Initialize(newCardDefinitions);
            _selectedCardsQueue.Clear();
            ScoreService.Reset();
            OnGameReset?.Invoke();
        }

        public void Reset(int newRows, int newColumns, IReadOnlyList<CardDefinition> newCardDefinitions, IReadOnlyList<CardState> newCardStates)
        {
            Board = new Board.Board(newRows, newColumns);
            Board.Load(newCardDefinitions, newCardStates);
            _selectedCardsQueue.Clear();
            for (int r = 0; r < newRows; r++)
            {
                for (int c = 0; c < newColumns; c++)
                {
                    if (Board.Cards[r, c].State == CardState.Revealed)
                        SelectCard(Board.Cards[r, c], true);
                }
            }
            OnGameReset?.Invoke();
        }

        public void SaveGame()
        {
            List<string> cardIds = Board.Cards
            .Cast<ICard>()
            .Select(card => card?.Definition?.Id ?? "") // used "" for empty slots
            .ToList();
            List<CardState> cardStates = Board.Cards
            .Cast<ICard>()
            .Select(card => card.State)
            .ToList();
            SaveLoadService.SaveGame(new GameData(Board.Rows, Board.Columns, ScoreService.Score, cardIds, cardStates));
        }

        public void LoadGame()
        {
            if (SaveLoadService.HasSavedGame)
            {
                GameData gameData = SaveLoadService.LoadGame();
                ScoreService.SetPoints(gameData.Score);
                int totalCards = gameData.CardIds.Count;
                CardDefinition[] cardDefinitions = new CardDefinition[totalCards];
                for (int i = 0; i < totalCards; ++i)
                {
                    CardDefinition cardDefinition = ScriptableObject.CreateInstance<CardDefinition>();
                    cardDefinition.Id = gameData.CardIds[i];
                    /*
                    for (int j = 0; j < _boardView.CardDefinitions.Length; ++j)
                    {
                        if (_boardView.CardDefinitions[j].Id == cardDefinition.Id)
                            cardDefinition.Sprite = _boardView.CardDefinitions[j].Sprite;
                    }
                    */
                    cardDefinitions[i] = cardDefinition;
                }
                Reset(gameData.Rows, gameData.Columns, cardDefinitions, gameData.CardStates);
            }
        }
    }
}