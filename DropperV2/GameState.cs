namespace DropperV2
{
    public class GameState
    {
        public int Rows { get; }
        public int Cols { get; }
        public GameGrid Grid { get; }
        public Direction Direction { get; private set; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; }
        public Player Player { get; private set; }

        public GameState(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Grid = new GameGrid(rows, cols);
            Direction = Direction.Right;

            AddPlayer();
        }

        private void AddPlayer()
        {
            Player = new Player(2);

            int startRow = Grid.Rows / 2 - Player.WidthHeight / 2;
            int startCol = Grid.Cols / 2 - Player.WidthHeight / 2;

            int i = 0;
            for (int r = startRow; r < startRow + Player.WidthHeight; r++)
            {
                for (int c = startCol; c < startCol + Player.WidthHeight; c++)
                {
                    Grid[r, c] = (int)GridValue.Player;
                    Player.PlayerTiles[i] = (new Position(r, c));
                    i++;
                }
            }
        }

        public bool PlayerCanFit()
        {
            foreach (Position p in Player.PlayerTiles)
                if (!Grid.EmptyCell(p))
                    return false;
            return true;
        }

        public void MovePlayerLeft()
        {
            //if(PlayerCanFit())

        }
    }
}
