global using System.Windows;
global using System.Windows.Controls;
global using System.Windows.Input;
global using System.Windows.Media;

namespace DropperV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool FormDragging = false;
        private Point DragOffset;

        private readonly Border[,] gridRects;
        private readonly Dictionary<GridValue, SolidColorBrush> gridValueToBackground;

        private GameState gameState;

        private readonly HashSet<Key> PressedKeys = [];

        private readonly Dictionary<GridValue, SolidColorBrush> gridValueToBorder;

        private void Form_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                FormDragging = true;

                var mouseLoc = PointToScreen(e.GetPosition(this));
                DragOffset = new Point(mouseLoc.X - Left, mouseLoc.Y - Top);
                Mouse.Capture((UIElement)sender);
            }
        }
        private void Form_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                FormDragging = false;
                Mouse.Capture(null);
            }
        }
        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (FormDragging)
                {
                    var screenLoc = PointToScreen(e.GetPosition(this));

                    Left = screenLoc.X - DragOffset.X;
                    Top = screenLoc.Y - DragOffset.Y;
                }
            }
        }
        private void TitleText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                if (sender is TextBlock tb)
                    tb.Foreground = QOL.RandomColor();
            }
            else if (e.ChangedButton == MouseButton.Left)
                Form_MouseDown(sender, e);
        }
        private void ClosingButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        public MainWindow()
        {
            InitializeComponent();

            gridValueToBackground = new()
            {
                { GridValue.Empty, FindResource("TileEmpty") as SolidColorBrush },
                { GridValue.Player, FindResource("TilePlayer") as SolidColorBrush },
                { GridValue.Enemy, FindResource("TileEnemy") as SolidColorBrush },
                { GridValue.Coin, FindResource("TileCoin") as SolidColorBrush },
                { GridValue.Wall, FindResource("TileWall") as SolidColorBrush }
            };

            gridValueToBorder = new()
            {
                {GridValue.Player, FindResource("TilePlayerBorder") as SolidColorBrush },
                {GridValue.Enemy, FindResource("TileEnemyBorder") as SolidColorBrush },
                //{GridValue.Player, FindResource("TilePlayerBorder") as SolidColorBrush },
                //{GridValue.Player, FindResource("TilePlayerBorder") as SolidColorBrush },
                //{GridValue.Player, FindResource("TilePlayerBorder") as SolidColorBrush },
            };

            TitleText.Foreground = QOL.RandomColor();
            TitleTextShadow.Foreground = QOL.RandomColor();

            gameState = new GameState(gridSize:16);
            gridRects = BuildGameGrid();
        }

        private Border[,] BuildGameGrid()
        {
            GameGrid.Rows = gameState.Grid.Rows;
            GameGrid.Columns = gameState.Grid.Cols;

            Border[,] rects = new Border[gameState.Grid.Rows, gameState.Grid.Cols];

            Brush emptyBrush = gridValueToBackground[GridValue.Empty];

            for (int r = 0; r < gameState.Grid.Rows; r++)
            {
                for (int c = 0; c < gameState.Grid.Cols; c++)
                {
                    Border cell = new()
                    {
                        Background = emptyBrush,
                        SnapsToDevicePixels = true
                    };

                    rects[r, c] = cell;
                    GameGrid.Children.Add(cell);
                }
            }
            return rects;
        }

        private void DrawGrid()
        {
            for (int r = 0; r < gameState.Grid.Rows; r++)
            {
                for (int c = 0; c < gameState.Grid.Cols; c++)
                {
                    GridValue gridValue = (GridValue)gameState.Grid[r, c];
                    gridRects[r, c].Background = gridValueToBackground[gridValue];

                    if (gridValue != (int)GridValue.Empty && gridValueToBorder.TryGetValue(gridValue, out SolidColorBrush entityBorderBrush))
                    {
                        Entity entity = gameState.EntityAtPos(r, c);
                        if (entity == null) return;

                        double top = (r == entity.CellTop) ? entity.BorderWidth : 0;
                        double bottom = (r == entity.CellBottom) ? entity.BorderWidth : 0;
                        double left = (c == entity.CellLeft) ? entity.BorderWidth : 0;
                        double right = (c == entity.CellRight) ? entity.BorderWidth : 0;

                        gridRects[r, c].BorderBrush = entityBorderBrush;
                        gridRects[r, c].BorderThickness = new Thickness(left, top, right, bottom);
                    }
                    else
                        gridRects[r, c].BorderThickness = new Thickness(0);
                }
            }
        }

        private void Draw()
        {
            DrawGrid();
        }

        private async Task GameLoop()
        {
            while (!gameState.GameOver)
            {
                HandleMovement();
                Draw();
                await Task.Delay(16);
            }
        }

        private async Task RunGame()
        {
            Draw();
            await GameLoop();
        }

        private async void GameGrid_Loaded(object sender, RoutedEventArgs e)
        {
            await RunGame();
        }

        private async void HandleMovement()
        {
            //if (PressedKeys.Contains(Key.W)) gameState.MovePlayerUp();
            if (PressedKeys.Contains(Key.A)) gameState.MovePlayerLeft();
            //if (PressedKeys.Contains(Key.S)) gameState.MovePlayerDown();
            if (PressedKeys.Contains(Key.D)) gameState.MovePlayerRight();

            if (PressedKeys.Contains(Key.K)) gameState.Grid.CenterEntity(gameState.Player, GridValue.Player);

            if (PressedKeys.Contains(Key.Y)) gameState.Grid.TopLeftEntity(gameState.Player, GridValue.Player);
            if (PressedKeys.Contains(Key.H)) gameState.Grid.TopRightEntity(gameState.Player, GridValue.Player);
            if (PressedKeys.Contains(Key.U)) gameState.Grid.BottomLeftEntity(gameState.Player, GridValue.Player);
            if (PressedKeys.Contains(Key.J)) gameState.Grid.BottomRightEntity(gameState.Player, GridValue.Player);

            if (PressedKeys.Contains(Key.I)) gameState.Grid.LeftCenterEntity(gameState.Player, GridValue.Player);
            if (PressedKeys.Contains(Key.O)) gameState.Grid.TopCenterEntity(gameState.Player, GridValue.Player);
            if (PressedKeys.Contains(Key.K)) gameState.Grid.BottomCenterEntity(gameState.Player, GridValue.Player);
            if (PressedKeys.Contains(Key.L)) gameState.Grid.RightCenterEntity(gameState.Player, GridValue.Player);

            if (PressedKeys.Contains(Key.Space)) gameState.Grid.EntityJump(gameState.Player, GridValue.Player);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) => PressedKeys.Add(e.Key);
        private void Window_KeyUp(object sender, KeyEventArgs e) => PressedKeys.Remove(e.Key);
    }
}