using Pathfinding_Visualizer.Models;
using Pathfinding_Visualizer.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Pathfinding_Visualizer.Algorithms
{
    public class DepthFirstSearch
    {
        private GridModel _gridModel;
        private Helpers _helpers;
        private AnimationController _animationController;
        private Stopwatch _stopwatch;

        public DepthFirstSearch(GridModel gridModel, Helpers helpers, AnimationController animationController, Stopwatch stopwatch)
        {
            _gridModel = gridModel;
            _helpers = helpers;
            _animationController = animationController;
            _stopwatch = stopwatch;
        }

        /// <summary>
        /// Performs a depth-first search on the grid
        /// </summary>
        /// <returns></returns>
        public async Task Run()
        {
            _stopwatch.Restart();

            Node? start = _helpers.GetStartNode();
            Node? end = _helpers.GetEndNode();

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

                foreach (Node neighbour in _helpers.GetNeighbours(current))
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

                        Border? square = _helpers.GetBorderForNode(neighbour);

                        if (square != null)
                            _gridModel.UpdateNodeColour(square);

                        await _animationController.WaitAsync();
                    }
                }
            }

            _stopwatch.Stop();
            _gridModel.DrawPath(parent, start, end);
        }
    }
}
