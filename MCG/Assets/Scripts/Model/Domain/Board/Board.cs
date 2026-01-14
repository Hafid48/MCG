using MCG.Model.Data;
using MCG.Model.Domain.Cards;
using System;
using System.Collections.Generic;

namespace MCG.Model.Domain.Board
{
    public class Board : IBoard
    {
        public int Rows { get; }
        public int Columns { get; }
        public ICard[,] Cards {  get; }

        public Board(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Cards = new ICard[rows, columns];
        }

        public void Initialize(IReadOnlyList<CardDefinition> definitions)
        {
            if (definitions == null || definitions.Count == 0)
                throw new ArgumentException("Card definitions cannot be null or empty");
            int cardsCount = Rows * Columns;
            int pairCount = cardsCount / 2;
            List<ICard> pool = new List<ICard>(cardsCount);
            for (int i = 0; i < pairCount; i++)
            {
                CardDefinition cardDefinition = definitions[i % definitions.Count];
                pool.Add(new Card(cardDefinition));
                pool.Add(new Card(cardDefinition));
            }
            Shuffle(pool);
            // Fill the board
            int index = 0;
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                    Cards[r, c] = index < pool.Count ? pool[index++] : new Card(null);
            }
        }

        public void Load(IReadOnlyList<CardDefinition> definitions, IReadOnlyList<CardState> cardStates)
        {
            int index = 0;
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    Cards[r, c] = index < cardStates.Count ? new Card(definitions[index]) : new Card(null);
                    Cards[r, c].State = cardStates[index];
                    index++;
                }
            }
        }

        public void Shuffle(IList<ICard> cards)
        {
            Random rng = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                --n;
                int k = rng.Next(n + 1);
                ICard temp = cards[k];
                cards[k] = cards[n];
                cards[n] = temp;
            }
        }

        public ICard GetCard(int row, int column)
        {
            if (!IsValidPosition(row, column))
                return null;
            return Cards[row, column];
        }

        private bool IsValidPosition(int row, int column)
        {
            return row >= 0 && row < Rows && column >= 0 && column < Columns;
        }
    }
}