using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DropperV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool FormDragging = false;
        private Point DragOffset;

        private int Rows = 24;
        private int Cols = 30;
        private Rectangle[,] gridRects;
        private Dictionary<GridValue, SolidColorBrush> gridValueToFill;
        private GameState gameState;


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

            gameState = new GameState(Rows, Cols);
            gridRects = BuildGameGrid();
        }

        private Rectangle[,] BuildGameGrid()
        {
            GameGrid.Rows = Rows;
            GameGrid.Columns = Cols;

            Rectangle[,] rects = new Rectangle[Rows, Cols];

            int cellSize = 32;
            Brush emptyBrush = gridValueToFill[GridValue.Empty];

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    Rectangle cell = new()
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Fill = emptyBrush,
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
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    GridValue gridValue = (GridValue)gameState.Grid[r, c];
                    gridRects[r, c].Fill = gridValueToFill[gridValue];
                }
            }
        }

        private void Draw()
        {
            DrawGrid();
            //TODO
        }

        private async Task GameLoop()
        {
            Draw();
            //TODO
        }

        private async Task RunGame()
        {
            Draw();
            await GameLoop();
            //TODO
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    //gameState.Player.tran
                    break;
            }
        }

        private async void GameGrid_Loaded(object sender, RoutedEventArgs e)
        {
            await RunGame();
        }
    }
}