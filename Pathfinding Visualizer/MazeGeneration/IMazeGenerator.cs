using System;
using System.Collections.Generic;
using System.Text;

namespace Pathfinding_Visualizer.MazeGeneration
{
    public interface IMazeGenerator
    {
        Task Generate();
    }
}
