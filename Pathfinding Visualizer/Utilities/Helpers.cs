using Pathfinding_Visualizer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Pathfinding_Visualizer.Utilities
{
    public class Helpers
    {
        private readonly UniformGrid _gridContainer;

        public Helpers(UniformGrid gridContainer)
        {
            _gridContainer = gridContainer;
        }

        /// <summary>
        /// Gets the start node from the grid
        /// </summary>
        /// <returns></returns>
        public Node? GetStartNode()
        {
            foreach (Border square in _gridContainer.Children)
            {
                Node node = (Node)square.Tag;

                if (node.State == NodeState.Start)
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the end node from the grid
        /// </summary>
        /// <returns></returns>
        public Node? GetEndNode()
        {
            foreach (Border square in _gridContainer.Children)
            {
                Node node = (Node)square.Tag;

                if (node.State == NodeState.End)
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the neighbours of a node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            foreach (Border square in _gridContainer.Children)
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

        /// <summary>
        /// Gets the border for a given node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public Border? GetBorderForNode(Node node)
        {
            foreach (Border square in _gridContainer.Children)
            {
                if (square.Tag == node)
                {
                    return square;
                }
            }

            return null;
        }

        /// <summary>
        /// Resets the distances of all nodes
        /// </summary>
        public void ResetNodeDistances()
        {
            foreach (Border square in _gridContainer.Children)
            {
                Node node = (Node)square.Tag;
                node.Distance = int.MaxValue;
                node.Heuristic = 0;
            }
        }

        /// <summary>
        /// Calculates the heuristic for a node based on its distance to the end node using Manhattan distance
        /// </summary>
        /// <param name="current"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int GetHeuristic(Node current, Node end)
        {
            return Math.Abs(current.Row - end.Row)
                 + Math.Abs(current.Column - end.Column);
        }
    }
}
