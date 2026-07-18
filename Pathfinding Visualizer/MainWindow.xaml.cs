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
        public MainWindow()
        {
            InitializeComponent();

            CreateGrid();
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

                    GridContainer.Children.Add(square);
                }
            }
        }
    }
}