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
            Player = new Player();
            CenterPlayer();
        }

        public void CenterPlayer()
        {
            Player.Move(11, 14);
        }

        public bool PlayerCanFit()
        {
            foreach (Position p in Player.TilePositions())
                if (!Grid.EmptyCell(p))
                    return false;
            return true;
        }


        public void MovePlayerUp()
        {
            Player.Move(-1, 0);
            if (!PlayerCanFit())
                Player.Move(1, 0);
        }

        public void MovePlayerLeft()
        {
            Player.Move(0, -1);
            if (!PlayerCanFit())
                Player.Move(0, 1);
        }

        public void MovePlayerDown()
        {
            Player.Move(1, 0);
            if (!PlayerCanFit())
                Player.Move(-1, 0);
        }

        public void MovePlayerRight()
        {
            Player.Move(0, 1);
            if (!PlayerCanFit())
                Player.Move(0, -1);
        }
    }
}
