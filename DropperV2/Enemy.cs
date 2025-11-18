using DropperV2;

public class Enemy : Entity
{
    public EnemyColor Color { get; } = EnemyColor.Default;

    public Enemy(int width = 2, int height = 2) : base(width, height)
    {
        Color = EnemyColor.Default;
    }
}

public enum EnemyColor
{
    Default,
    //TODO
}