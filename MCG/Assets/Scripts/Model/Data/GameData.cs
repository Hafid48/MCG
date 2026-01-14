using MCG.Model.Domain.Cards;
using System;
using System.Collections.Generic;

namespace MCG.Services
{
    [Serializable]
    public class GameData
    {
        public int Rows { get; }
        public int Columns { get; }
        public int Score { get; }
        public List<string> CardIds { get; }
        public List<CardState> CardStates { get; }

        public GameData(int rows, int columns, int score, List<string> cardIds, List<CardState> cardStates)
        {
            Rows = rows;
            Columns = columns;
            Score = score;
            CardIds = cardIds;
            CardStates = cardStates;
        }
    }
}