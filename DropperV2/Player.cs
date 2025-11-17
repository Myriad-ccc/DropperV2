using System.Security.Policy;

namespace DropperV2
{
    public class Player
    {
        public int Health { get; private set; }
        public int WidthHeight { get; }
        public readonly Position Offset; //implement offset logic for move
        public Position[] PlayerTiles { get; }
        public PlayerColor Color { get; }

        public Player(int widthHeight)
        {
            WidthHeight = widthHeight;
            Color = PlayerColor.Default;

            PlayerTiles = new Position[widthHeight * widthHeight];
        }
    }

    public enum PlayerColor
    {
        Default,
        //TODO
    }
}
