using Pathfinding_Visualizer.Models;
using Pathfinding_Visualizer.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pathfinding_Visualizer.MazeGeneration
{
    public class RecursiveBacktracking : IMazeGenerator
    {
        private readonly GridModel _gridModel;
        private readonly AnimationController _animationController;

        private readonly Random _random = new();

        Node[,] _nodes;

        public RecursiveBacktracking(GridModel gridModel, AnimationController animationController)
        {
            _gridModel = gridModel;
            _animationController = animationController;
        }

        public async Task Generate()
        {
            _gridModel.FillWithWalls();

            _nodes = _gridModel.GetNodes();

            Node start = _nodes[1, 1];

            start.State = NodeState.Empty;

            Stack<Node> stack = new();

            stack.Push(start);

            while (stack.Count > 0)
            {
                Node current = stack.Peek();

                List<Node> neighbours = GetUnvisitedNeighbours(current);

                if (neighbours.Count > 0)
                {
                    Node next = neighbours[_random.Next(neighbours.Count)];

                    int wallRow = (current.Row + next.Row) / 2;
                    int wallColumn = (current.Column + next.Column) / 2;

                    Node wall = _nodes[wallRow, wallColumn];

                    wall.State = NodeState.Empty;
                    next.State = NodeState.Empty;

                    stack.Push(next);

                    _gridModel.UpdateNodeColour(wall);
                    _gridModel.UpdateNodeColour(next);

                    await _animationController.WaitAsync();
                }
                else
                {
                    stack.Pop();
                }
            }
        }

        private List<Node> GetUnvisitedNeighbours(Node current)
        {
            List<Node> neighbours = new();

            int row = current.Row;
            int column = current.Column;

            // Up
            if (row - 2 >= 0)
            {
                Node neighbour = _nodes[row - 2, column];

                if (neighbour.State == NodeState.Wall)
                    neighbours.Add(neighbour);
            }

            // Down
            if (row + 2 < _gridModel.Rows)
            {
                Node neighbour = _nodes[row + 2, column];

                if (neighbour.State == NodeState.Wall)
                    neighbours.Add(neighbour);
            }

            // Left
            if (column - 2 >= 0)
            {
                Node neighbour = _nodes[row, column - 2];

                if (neighbour.State == NodeState.Wall)
                    neighbours.Add(neighbour);
            }

            // Right
            if (column + 2 < _gridModel.Columns)
            {
                Node neighbour = _nodes[row, column + 2];

                if (neighbour.State == NodeState.Wall)
                    neighbours.Add(neighbour);
            }

            return neighbours;
        }
    }
}
