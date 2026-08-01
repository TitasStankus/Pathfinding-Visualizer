using Pathfinding_Visualizer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Pathfinding_Visualizer.Utilities;
using System.Windows.Input;

namespace Pathfinding_Visualizer.Models
{
    public class GridModel
    {
        private readonly UniformGrid _gridContainer;

        private readonly MouseButtonEventHandler _leftClick;
        private readonly MouseButtonEventHandler _rightClick;
        private readonly MouseEventHandler _mouseEnter;

        private Helpers _helpers;

        private Node[,] _nodes;

        private int _rows = 20;
        private int _columns = 20;

        public int Rows
        {
            get => _rows;
            set => _rows = value;
        }

        public int Columns
        {
            get => _columns;
            set => _columns = value;
        }

        public GridModel(UniformGrid gridContainer, MouseButtonEventHandler leftClick, MouseButtonEventHandler rightClick, MouseEventHandler mouseEnter)
        {
            _gridContainer = gridContainer;
            _helpers = new Helpers(_gridContainer);
            _leftClick = leftClick;
            _rightClick = rightClick;
            _mouseEnter = mouseEnter;
        }

        /// <summary>
        /// Creates the grid of squares
        /// </summary>
        public void CreateGrid()
        {
            _gridContainer.Children.Clear();

            _gridContainer.Rows = _rows;    
            _gridContainer.Columns = _columns;

            _nodes = new Node[_rows, _columns];

            for (int row = 0; row < _rows; row++)
            {
                for (int column = 0; column < _columns; column++)
                {
                    Border square = new Border();

                    Node node = new Node(row, column);

                    square.Tag = node;

                    square.BorderBrush = Brushes.Gray;
                    square.BorderThickness = new Thickness(1);

                    UpdateNodeColour(square);

                    square.MouseLeftButtonDown += _leftClick;
                    square.MouseRightButtonDown += _rightClick;
                    square.MouseEnter += _mouseEnter;

                    _gridContainer.Children.Add(square);

                    _nodes[row, column] = node;
                }
            }
        }

        /// <summary>
        /// Updates the colour of a square based on its state
        /// </summary>
        /// <param name="square"></param>
        public void UpdateNodeColour(Border square)
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

        /// <summary>
        /// Changes the state of a square when clicked
        /// </summary>
        /// <param name="sender"></param>
        public void ChangeState(object sender)
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

        /// <summary>
        /// Draws the path from the end node to the start node using the parent dictionary
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public async void DrawPath(Dictionary<Node, Node> parent, Node start, Node end)
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

                    Border? square = _helpers.GetBorderForNode(current);

                    if (square != null)
                        UpdateNodeColour(square);

                    await Task.Delay(40);
                }

                current = parent[current];
            }
        }

        /// <summary>
        /// Resets the path and visited nodes to empty state
        /// </summary>
        public void ResetPath()
        {
            foreach (Border square in _gridContainer.Children)
            {
                Node node = (Node)square.Tag;

                if (node.State == NodeState.Visited || node.State == NodeState.Path)
                {
                    node.State = NodeState.Empty;
                    UpdateNodeColour(square);
                }
            }
        }

        public void SetDistances()
        {
            foreach (Border square in _gridContainer.Children)
            {
                Node node = (Node)square.Tag;

                node.Distance = int.MaxValue;
            }
        }
    }
}
