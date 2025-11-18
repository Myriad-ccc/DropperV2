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

        public void Clear(GridValue? type = null)
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                    if (type == null)
                        this[r, c] = (int)GridValue.Empty;
                    else
                        if (this[r, c] == (int)type)
                        this[r, c] = (int)GridValue.Empty;
        }

        public bool EntityCanFit(Entity entity)
        {
            foreach (Position p in entity.TilePositions())
                if (!EmptyCell(p))
                    return false;
            return true;
        }

        public void UpdateGridValue(Entity entity, GridValue type)
        {
            foreach (Position p in entity.TilePositions())
                this[p.Row, p.Col] = (int)type;
        }

        public void TryMoveEntityBy(Entity entity, GridValue type, int rowOffset, int colOffset)
        {
            Clear(type);

            entity.MoveBy(rowOffset, colOffset);
            if (!EntityCanFit(entity))
                entity.MoveBy(-rowOffset, -colOffset);

            UpdateGridValue(entity, type);
        }

        public void TryMoveEntityTo(Entity entity, GridValue type, int row, int col)
        {
            Clear(type);

            Position old = entity.Offset;

            entity.SetPos(row, col);
            if (!EntityCanFit(entity))
                entity.Offset = old;

            UpdateGridValue(entity, type);
        }
    }
}