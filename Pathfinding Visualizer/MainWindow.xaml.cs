using Pathfinding_Visualizer.Models;
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

        private bool _startSet;
        private bool _endSet;

        public MainWindow()
        {
            InitializeComponent();

            CreateGrid();

            MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;
        }

        private void CreateGrid()
        {
            _startSet = false;
            _endSet = false;

            GridContainer.Rows = _rows;
            GridContainer.Columns = _columns;

            for (int row=0; row < _rows; row++)
            {
                for (int column=0; column < _columns; column++)
                {
                    Border square = new Border();

                    Node node = new Node(row, column);

                    square.Tag = node;

                    square.BorderBrush = Brushes.Gray;
                    square.BorderThickness = new Thickness(1);

                    UpdateNodeColour(square);

                    square.MouseLeftButtonDown += Square_MouseLeftButtonDown;
                    square.MouseRightButtonDown += Square_MouseRightButtonDown;
                    square.MouseEnter += Square_MouseEnter;

                    GridContainer.Children.Add(square);
                }
            }
        }

        private void UpdateNodeColour(Border square)
        {
            Node node = (Node)square.Tag;

            switch (node.State)
            {
                case NodeState.Empty:
                    square.Background = Brushes.White;
                    break;
                case NodeState.Wall:
                    square.Background = Brushes.Black;
                    break;
                case NodeState.Start:
                    square.Background = Brushes.Green;
                    break;
                case NodeState.End:
                    square.Background = Brushes.Red;
                    break;
                case NodeState.Visited:
                    square.Background = Brushes.Blue;
                    break;
                case NodeState.Path:
                    square.Background = Brushes.Yellow;
                    break;
            }
        }

        private void ChangeState(object sender)
        {
            Border square = (Border)sender;

            Node node = (Node)square.Tag;

            if (node.State == NodeState.Wall)
            {
                node.State = NodeState.Empty;
            }
            else if (node.State == NodeState.Empty)
            {
                node.State = NodeState.Wall;
            }

            UpdateNodeColour(square);
        }

        private Node? GetStartNode()
        {
            foreach (Border square in GridContainer.Children)
            {
                Node node = (Node)square.Tag;

                if (node.State == NodeState.Start)
                {
                    return node;
                }
            }

            return null;
        }

        private Node? GetEndNode()
        {
            foreach (Border square in GridContainer.Children)
            {
                Node node = (Node)square.Tag;

                if (node.State == NodeState.End)
                {
                    return node;
                }
            }

            return null;
        }

        private List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            foreach (Border square in GridContainer.Children)
            {
                Node other = (Node)square.Tag;

                // Above
                if (other.Row == node.Row - 1 && other.Column == node.Column)
                    neighbours.Add(other);

                // Below
                if (other.Row == node.Row + 1 && other.Column == node.Column)
                    neighbours.Add(other);

                // Left
                if (other.Row == node.Row && other.Column == node.Column - 1)
                    neighbours.Add(other);

                // Right
                if (other.Row == node.Row && other.Column == node.Column + 1)
                    neighbours.Add(other);
            }

            return neighbours;
        }

        private Node GetNodeAt(int row, int column)
        {
            foreach (Border square in GridContainer.Children)
            {
                Node node = (Node)square.Tag;

                if (node.Row == row && node.Column == column)
                {
                    return node;
                }
            }

            return null;
        }

        private Border? GetBorderForNode(Node node)
        {
            foreach (Border square in GridContainer.Children)
            {
                if (square.Tag == node)
                {
                    return square;
                }
            }

            return null;
        }

        private void ResetNodeDistances()
        {
            foreach (Border square in GridContainer.Children)
            {
                Node node = (Node)square.Tag;
                node.Distance = int.MaxValue;
            }
        }

        private async void RunBFS_Click(object sender, RoutedEventArgs e)
        {
            await BreadthFirstSearch();
        }

        private async void RunDFS_Click(object sender, RoutedEventArgs e)
        {
            await DepthFirstSearch();
        }

        private async void RunDijkstra_Click(object sender, RoutedEventArgs e)
        {
            await Dijkstra();
        }

        private async Task BreadthFirstSearch()
        {
            Node? start = GetStartNode();
            Node? end = GetEndNode();

            if (start == null || end == null)
            {
                MessageBox.Show("Please place a Start and End node.");
                return;
            }

            Queue<Node> queue = new Queue<Node>();

            HashSet<Node> visited = new HashSet<Node>();

            Dictionary<Node, Node> parent = new Dictionary<Node, Node>();

            queue.Enqueue(start);

            visited.Add(start);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();

                if (current == end)
                    break;

                foreach (Node neighbour in GetNeighbours(current))
                {
                    if (visited.Contains(neighbour))
                        continue;

                    if (neighbour.State == NodeState.Wall)
                        continue;

                    visited.Add(neighbour);

                    parent[neighbour] = current;

                    queue.Enqueue(neighbour);

                    if (neighbour != end)
                    {
                        neighbour.State = NodeState.Visited;

                        Border? square = GetBorderForNode(neighbour);

                        if (square != null)
                            UpdateNodeColour(square);

                        await Task.Delay(20);
                    }
                }
            }

            DrawPath(parent, start, end);

        }

        private async Task DepthFirstSearch()
        {
            Node? start = GetStartNode();
            Node? end = GetEndNode();

            if (start == null || end == null)
            {
                MessageBox.Show("Please place a Start and End node.");
                return;
            }

            Stack<Node> stack = new Stack<Node>();

            HashSet<Node> visited = new HashSet<Node>();

            Dictionary<Node, Node> parent = new Dictionary<Node, Node>();

            stack.Push(start);

            visited.Add(start);

            while (stack.Count > 0)
            {
                Node current = stack.Pop();

                if (current == end)
                    break;

                foreach (Node neighbour in GetNeighbours(current))
                {
                    if (visited.Contains(neighbour))
                        continue;

                    if (neighbour.State == NodeState.Wall)
                        continue;

                    visited.Add(neighbour);

                    parent[neighbour] = current;

                    stack.Push(neighbour);

                    if (neighbour != end)
                    {
                        neighbour.State = NodeState.Visited;

                        Border? square = GetBorderForNode(neighbour);

                        if (square != null)
                            UpdateNodeColour(square);

                        await Task.Delay(20);
                    }
                }
            }

            DrawPath(parent, start, end);
        }

        private async Task Dijkstra()
        {
            Node? start = GetStartNode();
            Node? end = GetEndNode();

            if (start == null || end == null)
            {
                MessageBox.Show("Place a Start and End node.");
                return;
            }

            ResetPath_Click(null, null);
            ResetNodeDistances();

            foreach (Border square in GridContainer.Children)
            {
                Node node = (Node)square.Tag;

                node.Distance = int.MaxValue;
            }

            Dictionary<Node, Node> parent = new();

            HashSet<Node> visited = new();

            PriorityQueue<Node, int> queue = new();

            start.Distance = 0;

            queue.Enqueue(start, 0);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();

                if (visited.Contains(current))
                    continue;

                visited.Add(current);

                if (current == end)
                    break;

                foreach (Node neighbour in GetNeighbours(current))
                {
                    if (neighbour.State == NodeState.Wall)
                        continue;

                    int newDistance = current.Distance + 1;

                    if (newDistance < neighbour.Distance)
                    {
                        neighbour.Distance = newDistance;

                        parent[neighbour] = current;

                        queue.Enqueue(neighbour, neighbour.Distance);

                        if (neighbour != end)
                        {
                            neighbour.State = NodeState.Visited;

                            Border? square = GetBorderForNode(neighbour);

                            if (square != null)
                                UpdateNodeColour(square);

                            await Task.Delay(20);
                        }
                    }
                }
            }

            DrawPath(parent, start, end);
        }

        private async void DrawPath(Dictionary<Node, Node> parent, Node start, Node end)
        {
            if (!parent.ContainsKey(end))
            {
                MessageBox.Show("No path found.");
                return;
            }

            Node current = end;

            while (current != start)
            {
                if (current != end)
                {
                    current.State = NodeState.Path;

                    Border? square = GetBorderForNode(current);

                    if (square != null)
                        UpdateNodeColour(square);

                    await Task.Delay(40);
                }

                current = parent[current];
            }
        }

        private void Square_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = true;

            ChangeState(sender);
        }

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

            UpdateNodeColour(square);
        }

        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = false;
        }

        private void Square_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                ChangeState(sender);
            }
        }

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

            _rows = size;
            _columns = size;

            GridContainer.Children.Clear();

            CreateGrid();
        }

        private void ResetPath_Click(object sender, RoutedEventArgs e)
        {
            foreach (Border square in GridContainer.Children)
            {
                Node node = (Node)square.Tag;
                
                if (node.State == NodeState.Visited || node.State == NodeState.Path)
                {
                    node.State = NodeState.Empty;
                    UpdateNodeColour(square);
                }
            }
        }
    }
}