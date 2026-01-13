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

        public void Initialize()
        {
            
        }

        public void Shuffle()
        {
            
        }

        public ICard GetCard(int row, int column)
        {
            throw new System.NotImplementedException();
        }
    }
}