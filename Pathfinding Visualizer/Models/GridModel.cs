using System;
using System.Collections.Generic;
using System.Text;

namespace Pathfinding_Visualizer.Models
{
    public class GridModel
    {
        public Node[,] Nodes { get; }

        public int Rows { get; set; }
        public int Columns { get; set; }

        public GridModel(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            Nodes = new Node[rows, columns];

            CreateNodes();
        }

        private void CreateNodes()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    Nodes[row, column] = new Node(row, column);
                }
            }
        }
    }
}
