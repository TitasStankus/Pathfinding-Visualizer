using Pathfinding_Visualizer.Algorithms;
using Pathfinding_Visualizer.MazeGeneration;
using Pathfinding_Visualizer.Models;
using Pathfinding_Visualizer.Utilities;
using System.Diagnostics;
using System.Reflection.Emit;
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
        // Variables

        private GridModel _gridModel;

        private Helpers _helpers;

        private bool _isDrawing = false;

        private bool _startSet;
        private bool _endSet;

        private readonly AnimationController _animationController = new();

        // Constructor
        public MainWindow()
        {
            InitializeComponent();

            _gridModel = new GridModel(GridContainer, Square_MouseLeftButtonDown, Square_MouseRightButtonDown, Square_MouseEnter, _animationController);
            _gridModel.CreateGrid();

            _helpers = new Helpers(GridContainer);

            MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;
        }

        // ------------------------ Generation Buttons ------------------------

        private async void RunAlgorithm_Click(object sender, RoutedEventArgs e)
        {
            _animationController.Reset();

            Stopwatch stopwatch = Stopwatch.StartNew();

            switch (AlgorithmComboBox.SelectedIndex)
            {
                case 0:
                    BreadthFirstSearch bfs = new BreadthFirstSearch(_gridModel, _helpers, _animationController, stopwatch);

                    try
                    {
                        await bfs.Run();
                    }
                    catch
                    {
                        MessageBox.Show("Terminated");
                    }

                    break;

                case 1:
                    DepthFirstSearch dfs = new DepthFirstSearch(_gridModel, _helpers, _animationController, stopwatch);

                    try
                    {
                        await dfs.Run();
                    }
                    catch
                    {
                        MessageBox.Show("Terminated");
                    }

                    break;

                case 2:
                    Dijkstra dijkstra = new Dijkstra(_gridModel, _helpers, _animationController, stopwatch);

                    try
                    {
                        await dijkstra.Run();
                    }
                    catch
                    {
                        MessageBox.Show("Terminated");
                    }

                    break;

                case 3:
                    AStar aStar = new AStar(_gridModel, _helpers, _animationController, stopwatch);

                    try
                    {
                        await aStar.Run();
                    }
                    catch
                    {
                        MessageBox.Show("Terminated");
                    }

                    break;
            }

            TimeLabel.Text = stopwatch.ElapsedMilliseconds + " ms";
        }

        private async void GenerateMaze_Click(object sender, RoutedEventArgs e)
        {
            IMazeGenerator generator = null;

            switch (MazeComboBox.SelectedIndex)
            {
                case 0:
                    generator = new RandomWalls(_gridModel, _animationController);
                    break;
                case 1:
                    generator = new RecursiveBacktracking(_gridModel, _animationController);
                    break;
                case 2:
                    generator = new PrimsMaze(_gridModel, _animationController);
                    break;
            }

            await generator.Generate();
        }

        // ----------------------- Mouse Event Handlers -----------------------

        /// <summary>
        /// Handles the mouse left button down event on a square to start drawing walls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Square_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = true;

            _gridModel.ChangeState(sender);
        }

        /// <summary>
        /// Handles the mouse right button down event on a square to set the start or end node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Square_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border square = (Border)sender;

            Node node = (Node)square.Tag;

            if (!_startSet && node.State != NodeState.End)
            {
                node.State = NodeState.Start;
                _startSet = true;
            }
            else if (node.State == NodeState.Start)
            {
                node.State = NodeState.Empty;
                _startSet = false;
            }
            else if (node.State == NodeState.End)
            {
                node.State = NodeState.Empty;
                _endSet = false;
            }
            else if (!_endSet)
            {
                node.State = NodeState.End;
                _endSet = true;
            }

            _gridModel.UpdateNodeColour(square);
        }

        /// <summary>
        /// Handles the mouse left button up event on the main window to stop drawing walls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = false;
        }

        /// <summary>
        /// Handles the mouse enter event on a square to change its state if drawing is active
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Square_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                _gridModel.ChangeState(sender);
            }
        }

        // --------------------- Animation Event Handlers ---------------------

        /// <summary>
        /// Handles the click event on the Generate Grid button to create a new grid with the specified size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateGrid_Click(object sender, RoutedEventArgs e)
        {
            _startSet = false;
            _endSet = false;

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

            _gridModel.Rows = size;
            _gridModel.Columns = size;

            _gridModel.CreateGrid();
        }

        /// <summary>
        /// Handles the click event on the Reset Path button to clear the visited and path nodes from the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetPath_Click(object sender, RoutedEventArgs e)
        {
            _gridModel.ResetPath();
        }

        /// <summary>
        /// Handles the change of the animation speed slider to adjust the delay between animation steps
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnimationSpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_animationController == null) return;

            _animationController.animationDelay = 101 - (int)AnimationSpeedSlider.Value;
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            _animationController.Pause();
        }

        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            _animationController.Resume();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _animationController.Stop();
        }
    }
}