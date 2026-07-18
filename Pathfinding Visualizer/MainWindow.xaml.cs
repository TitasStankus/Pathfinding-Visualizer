using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pathfinding_Visualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _rows = 20;
        private int _columns = 20;

        private bool _isDrawing = false;

        public MainWindow()
        {
            InitializeComponent();

            CreateGrid();

            MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;
        }

        private void CreateGrid()
        {
            GridContainer.Rows = _rows;
            GridContainer.Columns = _columns;

            for (int row=0; row < _rows; row++)
            {
                for (int column=0; column < _columns; column++)
                {
                    Border square = new Border();

                    square.BorderBrush = Brushes.Gray;
                    square.BorderThickness = new Thickness(1);
                    square.Background = Brushes.White;

                    square.MouseLeftButtonDown += Square_MouseLeftButtonDown;
                    square.MouseEnter += Square_MouseEnter;

                    GridContainer.Children.Add(square);
                }
            }
        }

        private void Square_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = true;

            Border square = (Border)sender;
            square.Background = Brushes.Black;
        }

        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = false;
        }

        private void Square_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                Border square = (Border)sender;
                square.Background = Brushes.Black;
            }
        }

        private void GenerateGrid_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(GridSizeInput.Text, out int size))
            {
                MessageBox.Show("Please enter a valid grid size.");
                return;
            }

            if (size < 5 || size > 200)
            {
                MessageBox.Show("Please enter a grid size between 5 and 200.");
                return;
            }

            _rows = size;
            _columns = size;

            GridContainer.Children.Clear();

            CreateGrid();
        }
    }
}