namespace DropperV2
{
    public abstract class Entity
    {
        private int Health { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Position Offset { get; set; }

        private DateTime LastJump = DateTime.MinValue;
        public bool CanJump => (DateTime.Now - LastJump).Milliseconds >= 300;

        public Entity(int width = 2, int height = 2)
        {
            Width = width;
            Height = height;
            Offset = new Position(0, 0);
        }

        public void MoveBy(int rows, int cols)
        {
            Offset.Row += rows;
            Offset.Col += cols;
        }

        public void SetPos(int row, int col)
        {
            Offset.Row = row;
            Offset.Col = col;
        }

        public IEnumerable<Position> TilePositions()
        {
            for (int r = 0; r < Height; r++)
                for (int c = 0; c < Width; c++)
                    yield return new Position(Offset.Row + r, Offset.Col + c);
        }

        public async void StartJumpCooldown() => LastJump = DateTime.Now;
    }
}
