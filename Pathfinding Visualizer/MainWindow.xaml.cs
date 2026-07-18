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
        private bool _isDrawing = false;

        public MainWindow()
        {
            InitializeComponent();

            CreateGrid();

            MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;
        }

        private void CreateGrid()
        {
            for (int row=0; row < 20; row++)
            {
                for (int column=0; column < 20; column++)
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
    }
}