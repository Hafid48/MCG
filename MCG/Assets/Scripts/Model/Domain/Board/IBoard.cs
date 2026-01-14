using MCG.Model.Data;
using MCG.Model.Domain.Cards;
using System.Collections.Generic;

namespace MCG.Model.Domain.Board
{
    public interface IBoard
    {
        int Rows { get; }
        int Columns { get; }
        ICard[,] Cards { get; }

        void Initialize(IReadOnlyList<CardDefinition> definitions);

        void Load(IReadOnlyList<CardDefinition> definitions, IReadOnlyList<CardState> cardStates);

        void Shuffle(IList<ICard> cards);

        ICard GetCard(int row, int column);
    }
}