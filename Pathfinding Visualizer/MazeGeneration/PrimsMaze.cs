using System;
using System.Collections.Generic;
using System.Text;
using Pathfinding_Visualizer.Models;
using Pathfinding_Visualizer.Utilities;

namespace Pathfinding_Visualizer.MazeGeneration
{
    public class PrimsMaze : IMazeGenerator
    {
        private readonly GridModel _gridModel;
        private readonly AnimationController _animationController;

        Node[,] _nodes;

        private readonly Random _random = new();

        public PrimsMaze(GridModel gridModel, AnimationController animationController)
        {
            _gridModel = gridModel;
            _animationController = animationController;
        }

        public async Task Generate()
        {
            _nodes = _gridModel.GetNodes();

            _gridModel.FillWithWalls();

            Node start = _nodes[0, 0];

            start.State = NodeState.Empty;

            HashSet<Node> visited = new();
            visited.Add(start);

            List<Wall> walls = new();

            AddWalls(walls, start);

            while (walls.Count > 0)
            {
                int randomIndex = _random.Next(walls.Count);

                Wall wall = walls[randomIndex];

                walls.RemoveAt(randomIndex);

                if (visited.Contains(wall.Cell))
                {
                    continue;
                }

                wall.WallNode.State = NodeState.Empty;
                wall.Cell.State = NodeState.Empty;

                visited.Add(wall.Cell);

                _gridModel.UpdateNodeColour(wall.WallNode);
                _gridModel.UpdateNodeColour(wall.Cell);

                await _animationController.WaitAsync();

                AddWalls(walls, wall.Cell);
            }
        }

        private void AddWalls(List<Wall> walls, Node cell)
        {
            foreach (Wall wall in GetWalls(cell))
            {
                if (!walls.Any(w => w.WallNode == wall.WallNode))
                {
                    walls.Add(wall);
                }
            }
        }

        private List<Wall> GetWalls(Node cell)
        {
            List<Wall> walls = new();

            int row = cell.Row;
            int column = cell.Column;

            // Up
            if (row - 2 >= 0)
            {
                Node wall = _nodes[row - 1, column];
                Node nextCell = _nodes[row - 2, column];

                walls.Add(new Wall(wall, nextCell));
            }

            // Down
            if (row + 2 < _gridModel.Rows)
            {
                Node wall = _nodes[row + 1, column];
                Node nextCell = _nodes[row + 2, column];

                walls.Add(new Wall(wall, nextCell));
            }

            // Left
            if (column - 2 >= 0)
            {
                Node wall = _nodes[row, column - 1];
                Node nextCell = _nodes[row, column - 2];

                walls.Add(new Wall(wall, nextCell));
            }

            // Right
            if (column + 2 < _gridModel.Columns)
            {
                Node wall = _nodes[row, column + 1];
                Node nextCell = _nodes[row, column + 2];

                walls.Add(new Wall(wall, nextCell));
            }

            return walls;
        }
    }

    public class Wall
    {
        public Node WallNode { get; }
        public Node Cell { get; }

        public Wall(Node wallNode, Node cell)
        {
            WallNode = wallNode;
            Cell = cell;
        }
    }
}
