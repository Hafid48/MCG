namespace MCG.Model.Domain.Board
{
    public interface IBoard
    {
        int Rows { get; }
        int Columns { get; }
        ICard[,] Cards { get; }

        void Initialize();

        void Shuffle();

        ICard GetCard(int row, int column);
    }
}