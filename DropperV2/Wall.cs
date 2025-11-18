using DropperV2;

public class Wall : Entity
{
    public WallColor Color { get; } = WallColor.Default;

    public Wall(int width = 2, int height = 2) : base(width, height)
    {
        Color = WallColor.Default;
    }
}

public enum WallColor
{
    Default,
    //TODO
}