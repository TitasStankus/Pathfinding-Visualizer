using Pathfinding_Visualizer.Models;
using Pathfinding_Visualizer.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Pathfinding_Visualizer.MazeGeneration
{
    public class RandomWalls : IMazeGenerator
    {
        private readonly GridModel _gridModel;
        private readonly AnimationController _animationController;

        public RandomWalls(GridModel gridModel, AnimationController animationController)
        {
            _gridModel = gridModel;
            _animationController = animationController;
        }

        public Task Generate()
        {
            Node[,] nodes = _gridModel.GetNodes();

            foreach (Node node in nodes)
            {
                if (node.State == NodeState.Start || node.State == NodeState.End)
                {
                    continue;
                }

                if (new Random().NextDouble() < 0.3) // 30% chance to place a wall
                {
                    node.State = NodeState.Wall;
                    _gridModel.UpdateNodeColour(node);
                }
            }

            return Task.CompletedTask;
        }
    }
}
