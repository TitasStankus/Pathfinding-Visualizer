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
    public class Dijkstra
    {
        private GridModel _gridModel;
        private Helpers _helpers;
        private AnimationController _animationController;
        private Stopwatch _stopwatch;
        private readonly StatisticsManager _statisticsManager;

        public Dijkstra(GridModel gridModel, Helpers helpers, AnimationController animationController, Stopwatch stopwatch, StatisticsManager statisticsManager)
        {
            _gridModel = gridModel;
            _helpers = helpers;
            _animationController = animationController;
            _stopwatch = stopwatch;
            _statisticsManager = statisticsManager;
        }

        /// <summary>
        /// Performs Dijkstra's algorithm on the grid
        /// </summary>
        /// <returns></returns>
        public async Task Run()
        {
            _stopwatch.Restart();

            Node? start = _helpers.GetStartNode();
            Node? end = _helpers.GetEndNode();

            if (start == null || end == null)
            {
                MessageBox.Show("Place a Start and End node.");
                return;
            }

            _gridModel.ResetPath();
            _helpers.ResetNodeDistances();

            _gridModel.SetDistances();

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

                foreach (Node neighbour in _helpers.GetNeighbours(current))
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

                            Border? square = _helpers.GetBorderForNode(neighbour);

                            if (square != null)
                                _gridModel.UpdateNodeColour(square);

                            _statisticsManager.IncrementNodesVisited();

                            await _animationController.WaitAsync();
                        }
                    }
                }
            }

            _stopwatch.Stop();
            _statisticsManager.SetRuntime(_stopwatch.Elapsed);
            await _gridModel.DrawPath(parent, start, end);
        }
    }
}
