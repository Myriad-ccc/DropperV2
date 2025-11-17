namespace DropperV2
{
    public class GameGrid
    {
        private readonly int[,] grid;
        public int Rows { get; }
        public int Cols { get; }

        public int this[int r, int c]
        {
            get => grid[r, c];
            set => grid[r, c] = value;
        }

        public GameGrid(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            grid = new int[rows, cols];
        }

        public bool InGrid(Position position) =>
            position.Row >= 0 && position.Row < Rows &&
            position.Col >= 0 && position.Col < Cols;

        public bool EmptyCell(Position position) => InGrid(position) && grid[position.Row, position.Col] == 0;
    }
}