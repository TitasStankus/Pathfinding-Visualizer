using Pathfinding_Visualizer.Models;
using Pathfinding_Visualizer.Utilities;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Pathfinding_Visualizer.Algorithms
{
    public class BreadthFirstSearch
    {
        private GridModel _gridModel;
        private Helpers _helpers;
        private AnimationController _animationController;
        private Stopwatch _stopwatch;
        private readonly StatisticsManager _statisticsManager;

        public BreadthFirstSearch(GridModel gridModel, Helpers helpers, AnimationController animationController, Stopwatch stopwatch, StatisticsManager statisticsManager)
        {
            _gridModel = gridModel;
            _helpers = helpers;
            _animationController = animationController;
            _stopwatch = stopwatch;
            _statisticsManager = statisticsManager;
        }

        /// <summary>
        /// Performs a breadth-first search on the grid
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

                foreach (Node neighbour in _helpers.GetNeighbours(current))
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

                        Border? square = _helpers.GetBorderForNode(neighbour);

                        if (square != null)
                            _gridModel.UpdateNodeColour(square);

                        _statisticsManager.IncrementNodesVisited();

                        await _animationController.WaitAsync();
                    }
                }
            }

            _stopwatch.Stop();
            _statisticsManager.SetRuntime(_stopwatch.Elapsed);
            await _gridModel.DrawPath(parent, start, end);
        }
    }
}
