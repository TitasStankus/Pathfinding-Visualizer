using System;
using System.Collections.Generic;
using System.Text;

namespace Pathfinding_Visualizer.Models
{
    public class Node
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsWall { get; set; }
        public bool BeenVisited { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
    }
}
