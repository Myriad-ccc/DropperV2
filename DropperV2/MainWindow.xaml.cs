global using System.Windows;
global using System.Windows.Controls;
global using System.Windows.Input;
global using System.Windows.Media;
global using System.Windows.Shapes;

namespace DropperV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool FormDragging = false;
        private Point DragOffset;

        private readonly Rectangle[,] gridRects;
        private readonly Dictionary<GridValue, SolidColorBrush> gridValueToFill;
        private GameState gameState;

        private readonly HashSet<Key> PressedKeys = new();

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

            gridValueToFill = new()
            {
                { GridValue.Empty, FindResource("Tile.Empty") as SolidColorBrush },
                { GridValue.Player, FindResource("Tile.Player") as SolidColorBrush },
                { GridValue.Enemy, FindResource("Tile.Enemy") as SolidColorBrush },
                { GridValue.Coin, FindResource("Tile.Coin") as SolidColorBrush },
                { GridValue.Wall, FindResource("Tile.Wall") as SolidColorBrush }
            };
            TitleText.Foreground = QOL.RandomColor();
            TitleTextShadow.Foreground = QOL.RandomColor();

            gameState = new GameState(16);
            gridRects = BuildGameGrid();
        }

        private Rectangle[,] BuildGameGrid()
        {
            GameGrid.Rows = gameState.Grid.Rows;
            GameGrid.Columns = gameState.Grid.Cols;

            Rectangle[,] rects = new Rectangle[gameState.Grid.Rows, gameState.Grid.Cols];

            Brush emptyBrush = gridValueToFill[GridValue.Empty];
            Brush borderBrush = FindResource("Tile.Border") as SolidColorBrush;
            double borderThickness = 0.05;

            for (int r = 0; r < gameState.Grid.Rows; r++)
            {
                for (int c = 0; c < gameState.Grid.Cols; c++)
                {
                    Rectangle cell = new()
                    {
                        Fill = emptyBrush,
                        Stroke = borderBrush,
                        StrokeThickness = borderThickness,
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
                    gridRects[r, c].Fill = gridValueToFill[gridValue];
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

        private void HandleMovement()
        {
            if (PressedKeys.Contains(Key.W)) gameState.MovePlayerUp();
            if (PressedKeys.Contains(Key.A)) gameState.MovePlayerLeft();
            if (PressedKeys.Contains(Key.S)) gameState.MovePlayerDown();
            if (PressedKeys.Contains(Key.D)) gameState.MovePlayerRight();
            if (PressedKeys.Contains(Key.K)) gameState.CenterEntity(gameState.Player, GridValue.Player);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) => PressedKeys.Add(e.Key);
        private void Window_KeyUp(object sender, KeyEventArgs e) => PressedKeys.Remove(e.Key);
    }
}