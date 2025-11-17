using System.ComponentModel;

namespace DropperV2
{
    public class Player
    {
        public int Health { get; private set; }
        public PlayerColor Color { get; }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Position offset = new(0, 0);

        public Player(int width = 2, int height = 2)
        {
            Color = PlayerColor.Default;
            Width = width;
            Height = height;
        }

        public void Move(int rows, int cols)
        {
            offset.Row += rows;
            offset.Col += cols;
        }

        public IEnumerable<Position> TilePositions()
        {
            for (int r = 0; r < Height; r++)
                for (int c = 0; c < Width; c++)
                    yield return new Position(offset.Row + r, offset.Col + c);
        }
    }

    public enum PlayerColor
    {
        Default,
        //TODO
    }
}
