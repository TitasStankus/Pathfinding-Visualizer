using System;
using System.Collections.Generic;
using System.Text;

namespace Pathfinding_Visualizer.Models
{
    public class GridModel
    {
        public Node[,] Nodes { get; }

        public GridModel(int rows, int columns)
        {
            Nodes = new Node[rows, columns];
        }
    }
}
