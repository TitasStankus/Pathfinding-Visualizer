using System;
using System.Collections.Generic;
using System.Text;

namespace Pathfinding_Visualizer.Models
{
    public class Node
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public NodeState State { get; set; }

        public Node(int row, int column)
        {
            Row = row;
            Column = column;
            State = NodeState.Empty;
        }
    }
}
