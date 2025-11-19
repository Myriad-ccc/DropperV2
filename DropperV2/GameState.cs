namespace DropperV2
{
    public class GameState
    {
        private readonly Random random = new();

        public int Rows { get; }
        public int Cols { get; }
        public GameGrid Grid { get; }
        public Direction Direction { get; private set; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; }

        public Player Player { get; private set; }
        public Wall Wall { get; private set; }

        private List<Player> players = [];
        private List<Wall> enemies = [];

        public GameState(int gridSize = 1)
        {
            Rows = gridSize * 3;
            Cols = gridSize * 4;
            Grid = new GameGrid(Rows, Cols);
            Direction = Direction.Right;

            AddPlayer();
            AddWall();
        }

        private void AddPlayer()
        {
            Player = new Player(5, 5);
            players.Add(Player);
            Grid.CenterEntity(Player, GridValue.Player);
            Grid.UpdateGridValue(Player, GridValue.Player);
        }

        private void MovePlayer(int rowOffset, int colOffset) => Grid.TryMoveEntityBy(Player, GridValue.Player, rowOffset, colOffset);

        public void MovePlayerLeft() => MovePlayer(0, -1);
        public void MovePlayerRight() => MovePlayer(0, 1);
        public void MovePlayerUp() => MovePlayer(0, 0); //-1,0
        public void MovePlayerDown() => MovePlayer(0, 0); //1,0

        public void PlayerJump()
        {
            if (Player.CanJump)
            {
                MovePlayer(-4, 0);
                Player.StartJumpCooldown();
            }
        }

        private void AddWall()
        {
            Wall = new Wall(random.Next(2, 8), random.Next(1, 4));
            enemies.Add(Wall);

            do
                Wall.Offset = new Position(Player.Offset.Row + random.Next(-Rows / 2, Rows / 2), Player.Offset.Col + random.Next(-Cols / 2, Cols / 2));
            while (!Grid.EntityCanFit(Wall));

            Grid.UpdateGridValue(Wall, GridValue.Wall);
        }

    }
}
