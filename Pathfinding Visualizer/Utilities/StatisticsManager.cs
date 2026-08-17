using System;
using System.Collections.Generic;
using System.Text;

namespace Pathfinding_Visualizer.Utilities
{
    public class StatisticsManager
    {
        public string CurrentAlgorithm { get; private set; } = "None";

        public int NodesVisited { get; private set; }

        public int PathLength { get; private set; }

        public TimeSpan Runtime { get; private set; }

        public void Reset()
        {
            CurrentAlgorithm = "None";
            NodesVisited = 0;
            PathLength = 0;
            Runtime = TimeSpan.Zero;
        }

        public void SetAlgorithm(string algorithm)
        {
            CurrentAlgorithm = algorithm;
        }

        public void IncrementNodesVisited()
        {
            NodesVisited++;
        }

        public void SetPathLength(int length)
        {
            PathLength = length;
        }

        public void SetRuntime(TimeSpan runtime)
        {
            Runtime = runtime;
        }
    }
}
