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

        public void Clear()
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                    this[r, c] = (int)GridValue.Empty;
        }

        public void ClearPlayer()
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                    if (this[r, c] == (int)GridValue.Player)
                        this[r, c] = (int)GridValue.Empty;
        }
    }
}